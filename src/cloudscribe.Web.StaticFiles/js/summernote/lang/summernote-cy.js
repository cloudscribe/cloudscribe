/*!
 * 
 * Super simple WYSIWYG editor v0.9.0
 * https://summernote.org
 *
 * Copyright 2024 - ESDM an Idox Company
 * Summernote may be freely distributed under the MIT license.
 *
 * Date: 2024-09-30T14:42Z
 *
 */
(function webpackUniversalModuleDefinition(root, factory) {
	if(typeof exports === 'object' && typeof module === 'object')
		module.exports = factory(require("jquery"));
	else if(typeof define === 'function' && define.amd)
		define(["jquery"], factory);
	else {
		var a = typeof exports === 'object' ? factory(require("jquery")) : factory(root["jQuery"]);
		for(var i in a) (typeof exports === 'object' ? exports : root)[i] = a[i];
	}
})(self, (__WEBPACK_EXTERNAL_MODULE__8938__) => {
return /******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ 8938:
/***/ ((module) => {

module.exports = __WEBPACK_EXTERNAL_MODULE__8938__;

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => (module['default']) :
/******/ 				() => (module);
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(8938);
/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);

(jquery__WEBPACK_IMPORTED_MODULE_0___default().summernote) = (jquery__WEBPACK_IMPORTED_MODULE_0___default().summernote) || {
  lang: {}
};
jquery__WEBPACK_IMPORTED_MODULE_0___default().extend(true, (jquery__WEBPACK_IMPORTED_MODULE_0___default().summernote).lang, {
  'cy': {
    font: {
      bold: 'Beiddgar',
      italic: 'Italig',
      underline: 'Tanlinellu',
      clear: 'Dileu Arddull Ffont',
      height: 'Uchder y Llinell',
      name: 'Teulu Ffont',
      strikethrough: 'Streic drwodd',
      subscript: 'Tanysgrifiad',
      superscript: 'Uwchysgrif',
      size: 'Maint Ffont',
      sizeunit: 'Uned Maint Ffont'
    },
    image: {
      image: 'Llun',
      insert: 'Mewnosod Delwedd',
      resizeFull: 'Newid maint llawn',
      resizeHalf: 'Newid maint hanner',
      resizeQuarter: 'Newid maint chwarter',
      resizeNone: 'Maint gwreiddiol',
      floatLeft: 'Arnofio Chwith',
      floatRight: 'Arnofio i\'r Dde',
      floatNone: 'Tynnwch fflôt',
      shapeRounded: 'Siâp: Talgrynnu',
      shapeCircle: 'Siâp: Cylch',
      shapeThumbnail: 'Siâp: Mân-lun',
      shapeNone: 'Siâp: Dim',
      dragImageHere: 'Llusgwch ddelwedd neu destun yma',
      dropImage: 'Gollwng delwedd neu Testun',
      selectFromFiles: 'Dewiswch o ffeiliau',
      maximumFileSize: 'Maint ffeil mwyaf',
      maximumFileSizeError: 'Wedi mynd y tu hwnt i uchafswm maint y ffeil.',
      url: 'URL delwedd',
      remove: 'Dileu Delwedd',
      original: 'Gwreiddiol'
    },
    video: {
      video: 'Fideo',
      videoLink: 'Cyswllt Fideo',
      insert: 'Mewnosod Fideo',
      url: 'URL fideo',
      providers: '(YouTube, Google Drive, Vimeo, Vine, Instagram, DailyMotion, Youku, Peertube)'
    },
    link: {
      link: 'Cyswllt',
      insert: 'Mewnosod Dolen',
      unlink: 'Datgysylltu',
      edit: 'Golygu',
      textToDisplay: 'Testun i\'w arddangos',
      url: 'I ba URL ddylai\'r ddolen hon fynd?',
      openInNewWindow: 'Agor mewn ffenestr newydd'
    },
    table: {
      table: 'Tabl',
      addRowAbove: 'Ychwanegu rhes uchod',
      addRowBelow: 'Ychwanegu rhes isod',
      addColLeft: 'Ychwanegu colofn i\'r chwith',
      addColRight: 'Ychwanegu colofn ar y dde',
      delRow: 'Dileu rhes',
      delCol: 'Dileu colofn',
      delTable: 'Dileu tabl'
    },
    hr: {
      insert: 'Mewnosod Rheol Llorweddol'
    },
    style: {
      style: 'Arddull',
      p: 'Arferol',
      blockquote: 'Dyfyniad',
      pre: 'Cod',
      h1: 'Pennawd 1',
      h2: 'Pennawd 2',
      h3: 'Pennawd 3',
      h4: 'Pennawd 4',
      h5: 'Pennawd 5',
      h6: 'Pennawd 6'
    },
    lists: {
      unordered: 'Rhestr heb ei threfnu',
      ordered: 'Rhestr wedi\'i harchebu'
    },
    options: {
      help: 'Help',
      fullscreen: 'Sgrin Lawn',
      codeview: 'Gweld Cod'
    },
    paragraph: {
      paragraph: 'Paragraff',
      outdent: 'Allanol',
      indent: 'Mewnoliad',
      left: 'Alinio i\'r chwith',
      center: 'Alinio canol',
      right: 'Alinio i\'r dde',
      justify: 'Cyfiawnhau yn llawn'
    },
    color: {
      recent: 'Lliw Diweddar',
      more: 'Mwy o liw',
      background: 'Lliw Cefndir',
      foreground: 'Lliw Testun',
      transparent: 'Tryloyw',
      setTransparent: 'Gosod Tryloyw',
      reset: 'Ailosod',
      resetToDefault: 'Ailosod i ddiofyn',
      cpSelect: 'Dewiswch'
    },
    shortcut: {
      shortcuts: 'Llwybrau byr bysellfwrdd',
      close: 'Cau',
      textFormatting: 'Fformatio testun',
      action: 'Gweithred',
      paragraphFormatting: 'Fformatio paragraff',
      documentStyle: 'Arddull Dogfen',
      extraKeys: 'Allweddi ychwanegol'
    },
    help: {
      'escape': 'Dianc',
      'insertParagraph': 'Mewnosodwch Baragraff',
      'undo': 'Dad-wneud y gorchymyn olaf',
      'redo': 'Ail-wneud y gorchymyn olaf',
      'tab': 'Tab',
      'untab': 'Untab',
      'bold': 'Gosodwch arddull feiddgar',
      'italic': 'Gosodwch arddull italig',
      'underline': 'Gosodwch arddull tanlinellu',
      'strikethrough': 'Gosodwch arddull taro trwodd',
      'removeFormat': 'Glanhewch arddull',
      'justifyLeft': 'Gosod aliniad chwith',
      'justifyCenter': 'Gosod aliniad canol',
      'justifyRight': 'Gosod aliniad dde',
      'justifyFull': 'Gosod aliniad llawn',
      'insertUnorderedList': 'Toglo rhestr heb ei threfnu',
      'insertOrderedList': 'Toglo rhestr archebedig',
      'outdent': 'Allanol ar y paragraff cyfredol',
      'indent': 'Indent ar y paragraff cyfredol',
      'formatPara': 'Newid fformat y bloc cyfredol fel paragraff (tag P)',
      'formatH1': 'Newid fformat y bloc cyfredol fel H1',
      'formatH2': 'Newid fformat y bloc cyfredol fel H2',
      'formatH3': 'Newid fformat y bloc cyfredol fel H3',
      'formatH4': 'Newid fformat y bloc cyfredol fel H4',
      'formatH5': 'Newid fformat y bloc cyfredol fel H5',
      'formatH6': 'Newid fformat y bloc cyfredol fel H6',
      'insertHorizontalRule': 'Mewnosod rheol lorweddol',
      'linkDialog.show': 'Dangos Deialog Cyswllt'
    },
    history: {
      undo: 'Dadwneud',
      redo: 'Ail-wneud'
    },
    specialChar: {
      specialChar: 'CYMERIADAU ARBENNIG',
      select: 'Dewiswch nodau arbennig'
    },
    output: {
      noSelection: 'Dim Dewis Wedi\'i Wneud!'
    }
  }
});
/******/ 	return __webpack_exports__;
/******/ })()
;
});