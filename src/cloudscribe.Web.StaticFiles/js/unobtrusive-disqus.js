(function () {
    document.addEventListener("DOMContentLoaded", function () {
        var discusElement = document.querySelector('[data-disqus-config]');
        if (discusElement) {
            var pageUrl = discusElement.dataset.disqusPageUrl;
            var pageId = discusElement.dataset.disqusPageId;
            var scriptUrl = discusElement.dataset.disqusScriptUrl;
            if (pageUrl && pageId && scriptUrl) {
                var disqus_config = function () {
                    this.page.url = pageUrl;
                    this.page.identifier = pageId;
                };
                var d = document, s = d.createElement('script');
                s.src = scriptUrl;
                s.setAttribute('data-timestamp', +new Date());
                (d.head || d.body).appendChild(s);
            }
        }
    });
})();