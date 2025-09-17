/**
 * Summernote Element Path Plugin
 * Displays the current element path (like body > h2 > strong) in the status bar
 * Similar to CKEditor/TinyMCE element path display
 * copyright 2024 ESDM, an Idox Company.
 * 
 * @summary Summernote Element Path Plugin
 * @author Jim K - Idox plc.
 * @license MIT
 */

(function (factory) {
	// Universal Module Definition
	if (typeof define === 'function' && define.amd) {
		// AMD. Register as an anonymous module.
		define(['jquery'], factory);
	} else if (typeof module === 'object' && module.exports) {
		// Node/CommonJS
		module.exports = factory(require('jquery'));
	} else {
		// Browser globals
		factory(window.jQuery);
	}
}(function ($) {

	// Extend Summernote Language
	$.extend(true, $.summernote.lang, {
		'en-US': {
			elementPath: {
				tooltip: 'Element Path'
			}
		}
	});

	// Extend Summernote Plugins
	$.extend($.summernote.plugins, {
		/**
		 * @param {Object} context - Summernote context
		 */
		'elementpath': function (context) {
			var self = this;
			var ui = $.summernote.ui;
			var $editor = context.layoutInfo.editor;
			var $editable = context.layoutInfo.editable;
			var $statusbar = context.layoutInfo.statusbar;
			var options = context.options;
			var lang = options.langInfo;
			
			// Default options
			var defaultOptions = {
				enabled: true,
				separator: '  ',
				clickToSelect: true,
				showTags: ['P', 'H1', 'H2', 'H3', 'H4', 'H5', 'H6', 'BLOCKQUOTE', 
				           'PRE', 'LI', 'OL', 'UL', 'TD', 'TH', 'TR', 'TABLE', 'DIV', 
				           'STRONG', 'B', 'EM', 'I', 'U', 'S', 'STRIKE', 'SUP', 'SUB', 'A', 'CODE', 'SPAN',
				           'SMALL', 'CITE', 'CAPTION', 'THEAD', 'TBODY', 'TFOOT', 'ARTICLE', 'DL', 'DT', 'DD',
				           'HEADER', 'FOOTER', 'MAIN', 'SECTION', 'ASIDE', 'NAV', 'FIGURE', 'FIGCAPTION', 
				           'MARK', 'HR', 'FIELDSET', 'LEGEND', 'LABEL', 'ADDRESS', 'INPUT',
				           'DETAILS', 'SUMMARY', 'IFRAME', 'AUDIO', 'VIDEO', 'SOURCE', 'DIALOG', 
				           'PROGRESS', 'METER', 'OUTPUT', 'CANVAS', 'SVG']
			};
			
			// Merge options
			var elementPathOptions = $.extend({}, defaultOptions, options.elementPath || {});
			
			// Create element path container
			var $elementPath = null;
			
			/**
			 * Initialize plugin
			 */
			this.initialize = function () {
				console.log('[ElementPath] Initializing plugin, enabled:', elementPathOptions.enabled);
				// console.log('[ElementPath] $statusbar:', $statusbar, 'length:', $statusbar.length);
				// console.log('[ElementPath] $editable:', $editable[0]);
				
				if (!elementPathOptions.enabled) return;
				
				// Add element path container to status bar
				if ($statusbar.length > 0) {
					// Check if this statusbar already has an element path (for this specific instance)
					if ($statusbar.find('.note-element-path').length > 0) {
						// console.log('[ElementPath] Element path already exists for this editor, skipping...');
						return;
					}
					
					// Check if status output area exists, if not create it
					var $statusOutput = $statusbar.find('.note-status-output');
					if ($statusOutput.length === 0) {
						$statusOutput = $('<div class="note-status-output"></div>');
						$statusbar.prepend($statusOutput);
					}
					
					// Create element path display with unique identifier
					var editorId = $editable.attr('id') || Math.random().toString(36).substr(2, 9);
					$elementPath = $('<div class="note-element-path" data-editor-id="' + editorId + '"></div>');
					$elementPath.css({
						'display': 'inline-block',
						'margin-right': '10px',
						'padding-left': '8px',
						'padding-top': '5px',
						'color': '#666',
						'font-size': '12px'
					});
					$statusOutput.append($elementPath);
					
					// console.log('[ElementPath] Created element path for editor:', editorId);
					
					// Bind events
					this.bindEvents();
				}
			};
			
			/**
			 * Get the element path from current selection
			 */
			this.getElementPath = function () {
				var path = [];
				var selection = window.getSelection();
				
				// console.log('[ElementPath] Getting element path...');
				// console.log('[ElementPath] Selection rangeCount:', selection.rangeCount);
				
				if (selection.rangeCount > 0) {
					var range = selection.getRangeAt(0);
					var node = range.startContainer;
					
					// console.log('[ElementPath] Initial node:', node);
					// console.log('[ElementPath] Node is inside editable?', $editable[0].contains(node));
					
					// Make sure we're inside the editor
					if (!$editable[0].contains(node)) {
						// console.log('[ElementPath] Selection is outside editor, checking if editor has focus...');
						// Try to get the last known position in the editor
						var savedRange = context.invoke('editor.getLastRange');
						// console.log('[ElementPath] Saved range:', savedRange);
						if (savedRange && savedRange.sc) {
							node = savedRange.sc;
						} else {
							return path;
						}
					}
					
					// If it's a text node, get the parent element
					if (node.nodeType === 3) {
						node = node.parentElement;
					}
					
					// console.log('[ElementPath] After text node check:', node);
					// console.log('[ElementPath] $editable[0]:', $editable[0]);
					
					// Build path from current node to editable root
					while (node && node !== $editable[0] && !$(node).hasClass('note-editable')) {
						var nodeName = node.nodeName.toUpperCase();
						
						// console.log('[ElementPath] Checking node:', nodeName, 'in showTags:', elementPathOptions.showTags);
						
						// Only add if it's in the showTags list
						if (elementPathOptions.showTags.indexOf(nodeName) !== -1) {
							// Create element info object
							var elementInfo = {
								node: node,
								name: nodeName.toLowerCase(),
								displayName: nodeName.toLowerCase()
							};
							
							// Add class or id for better identification
							if (node.id) {
								elementInfo.displayName += '#' + node.id;
							} else if (node.className && typeof node.className === 'string') {
								var classes = node.className.split(' ').filter(function(c) {
									return c && !c.startsWith('note-'); // Exclude Summernote classes
								});
								if (classes.length > 0) {
									elementInfo.displayName += '.' + classes[0];
								}
							}
							
							path.unshift(elementInfo);
						}
						
						node = node.parentElement;
					}
				}
				
				return path;
			};
			
			/**
			 * Update the element path display
			 */
			this.updateDisplay = function () {
				// console.log('[ElementPath] updateDisplay called, $elementPath:', $elementPath);
				if (!$elementPath) return;
				
				// Hide element path content in source code mode but maintain space
				if ($editor.hasClass('codeview')) {
					$elementPath.css('visibility', 'hidden');
					return;
				} else {
					$elementPath.css('visibility', 'visible');
				}
				
				var path = this.getElementPath();
				// console.log('[ElementPath] Path found:', path);
				
				// Always start with "body" as the base
				var pathHtml = ['<span style="color: #999; padding-right: 4px;">body</span>'];
				
				// Add elements from the path
				if (path.length > 0) {
					path.forEach(function (element, index) {
						if (elementPathOptions.clickToSelect) {
							// Make elements clickable
							pathHtml.push('<a href="#" class="note-element-path-item" data-element-index="' + 
							              index + '" style="color: #303030; font-weight: 600; text-decoration: none; padding: 2px 4px; ' +
							              'border-radius: 2px;" onmouseover="this.style.backgroundColor=\'#e0e8f0\'" ' +
							              'onmouseout="this.style.backgroundColor=\'transparent\'">' + 
							              element.displayName + '</a>');
						} else {
							pathHtml.push('<span style="color: #303030; font-weight: 600;">' + element.displayName + '</span>');
						}
					});
				}
				
				$elementPath.html(pathHtml.join('<span style="color: #999;">' + 
				                                 elementPathOptions.separator + '</span>'));
				
				// Attach click handlers if enabled
				if (elementPathOptions.clickToSelect) {
					$elementPath.find('.note-element-path-item').on('click', function (e) {
						e.preventDefault();
						// console.log('[ElementPath] Element path item clicked');
						var index = parseInt($(this).data('element-index'));
						// console.log('[ElementPath] Clicked index:', index, 'Path length:', path.length);
						if (index >= 0 && index < path.length) {
							// console.log('[ElementPath] Selecting element:', path[index]);
							self.selectElement(path[index].node);
						} else {
							// console.error('[ElementPath] Invalid index:', index);
						}
					});
				}
			};
			
			/**
			 * Select an element in the editor
			 */
			this.selectElement = function (element) {
				// console.log('[ElementPath] selectElement called with:', element);
				if (!element) {
					// console.log('[ElementPath] No element provided to selectElement');
					return;
				}
				
				try {
					// Focus the editor first
					$editable.focus();
					
					// Create a range that selects the entire element
					var range = document.createRange();
					range.selectNodeContents(element);
					// console.log('[ElementPath] Created range:', range);
					
					// Apply the selection
					var selection = window.getSelection();
					selection.removeAllRanges();
					selection.addRange(range);
					// console.log('[ElementPath] Applied selection, rangeCount:', selection.rangeCount);
					
					// Notify Summernote of the range change (but don't trigger events that cause loops)
					context.invoke('editor.saveRange');
					
					// console.log('[ElementPath] Selection completed');
					
				} catch (error) {
					// console.error('[ElementPath] Error in selectElement:', error);
				}
			};
			
			/**
			 * Bind events for updating element path
			 */
			this.bindEvents = function () {
				// Update on various editor events - bind to the specific editable area
				var updateEvents = [
					'keyup',
					'mouseup',
					'focus',
					'blur'
				];
				
				// Bind events directly to the editable area for this instance
				updateEvents.forEach(function (eventName) {
					$editable.on(eventName, function (e) {
						// console.log('[ElementPath] Event triggered:', eventName, 'for editor:', $editable[0]);
						// Add delay for mouseup to ensure selection has updated
						if (eventName === 'mouseup') {
							setTimeout(function() {
								self.updateDisplay();
							}, 10);
						} else {
							self.updateDisplay();
						}
					});
				});
				
				// Add immediate click handler for faster response to selection changes
				$editable.on('click', function (e) {
					// console.log('[ElementPath] Click event triggered for editor:', $editable[0]);
					setTimeout(function() {
						self.updateDisplay();
					}, 20);
				});
				
				// Also bind to editor-level summernote events
				var summernoteEvents = [
					'summernote.keyup',
					'summernote.mouseup', 
					'summernote.change'
				];
				
				summernoteEvents.forEach(function (eventName) {
					$editor.on(eventName, function () {
						// console.log('[ElementPath] Summernote event triggered:', eventName);
						self.updateDisplay();
					});
				});
				
				// Bind to toolbar button clicks for immediate formatting updates
				$editor.on('click', '.note-toolbar .note-btn', function(e) {
					// console.log('[ElementPath] Toolbar button clicked:', $(this));
					// Small delay to allow formatting to be applied before updating
					setTimeout(function() {
						self.updateDisplay();
					}, 50);
				});
				
				// Bind to dropdown menu item clicks (for style dropdown H1, H2, etc.)
				$editor.on('click', '.dropdown-style .dropdown-item', function(e) {
					// console.log('[ElementPath] Style dropdown item clicked:', $(this), 'data-value:', $(this).data('value'));
					// Longer delay for dropdown selections as they may take more time to apply
					setTimeout(function() {
						self.updateDisplay();
					}, 100);
				});
				
				// Also listen for format changes via summernote events
				var formatEvents = [
					'summernote.paste',
					'summernote.enter'
				];
				
				formatEvents.forEach(function (eventName) {
					$editor.on(eventName, function () {
						// console.log('[ElementPath] Format event triggered:', eventName);
						setTimeout(function() {
							self.updateDisplay();
						}, 100);
					});
				});
				
				// Initial update with delay to ensure editor is ready
				setTimeout(function () {
					// console.log('[ElementPath] Initial update for editor:', $editable[0]);
					self.updateDisplay();
				}, 100);
			};
			
			/**
			 * Destroy the plugin
			 */
			this.destroy = function () {
				if ($elementPath) {
					$elementPath.remove();
					$elementPath = null;
				}
			};
		}
	});
}));