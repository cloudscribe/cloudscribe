
https://polymer.slack.com/messages/C2NH75P0D/


## Coding Style Guides

https://news.ycombinator.com/item?id=13159251

nested routing
http://stackoverflow.com/questions/38622697/polymer-nested-app-routes-are-not-mapping-correctly

### PRPL Pattern
Push - pro-actively deliver all the resources required for first view (http2/push)
Render (only the requested route)
Pre-Cache - service worker
L - Lazy Load

app shell architecture
https://developers.google.com/web/updates/2015/11/app-shell

A straightforward way to architect Polymer application
https://google.github.io/uniflow-polymer/
https://github.com/google/uniflow-polymer


AppToolbox 
https://www.polymer-project.org/2.0/toolbox/
sample apps
https://www.polymer-project.org/2.0/toolbox/news-case-study
https://www.polymer-project.org/2.0/toolbox/case-study



https://www.polymer-project.org/blog/2017-05-15-time-for-two

##### google IO 2017
Future, Faster: Unlock the Power of Web Components with Polymer
https://www.youtube.com/watch?v=cuoZenpQveQ&list=PLNYkxOF6rcICniLJ2rfj0FexlA-9zmJJE

samples
http://aspiring-chauffeur.glitch.me/

http://react-etc.net/entry/why-is-google-working-on-angular-2-and-polymer

https://mentormate.com/blog/polymer-vs-angular-future-web-apps/

https://dmitriid.com/blog/2017/03/the-broken-promise-of-web-components/

https://robdodson.me/regarding-the-broken-promise-of-web-components/

https://medium.com/@velmu/go-long-on-web-components-b1e0689f64e4


https://developers.google.com/web/fundamentals/

https://developers.google.com/web/fundamentals/performance/prpl-pattern/

https://www.polymer-project.org/1.0/

https://customelements.io/
https://vaadin.com/elements

http://thewebplatform.libsyn.com/webpage

https://webkit.org/blog/7027/introducing-custom-elements/

http://developer.telerik.com/featured/what-to-expect-from-javascript-in-2016-frameworks/

http://mentormate.com/blog/polymer-vs-angular-future-web-apps/

https://home-assistant.io/blog/2016/05/18/why-we-use-polymer/

https://optimizely.github.io/nuclear-js/

http://www.fiyazhasan.me/tag/polymer-series/

http://www.fiyazhasan.me/building-apps-with-polymer-and-asp-net-core-part-iii/

polymer group
https://groups.google.com/forum/#!forum/polymer-dev

https://developer.microsoft.com/en-us/microsoft-edge/platform/status/

https://javascriptair.com/episodes/2016-11-02/

https://blog.intercom.com/browsers-not-apps-are-the-future-of-mobile/

How to lazy load script dependencies
https://www.tjvantoll.com/2014/08/12/the-problem-with-using-html-imports-for-dependency-management/
https://gist.github.com/wycats/51c96e3adcdb3a68cbc3
https://github.com/Polymer/polymer/issues/3875
Fast Polymer app loading
https://gist.github.com/ebidel/1ba71473d687d0567bd3

https://www.polymer-project.org/1.0/docs/release-notes

## Testing

using the shop app sample, when I run polymer test, which internally uses selenium, it fails with an error saying java is not installed in your path, please install java.

I don't want to install effing java on my machine.

apparently it is using the java web driver
but it looks like maybe c# bindings can be used instead
http://www.seleniumhq.org/docs/03_webdriver.jsp

need to investigate whether that is possible
http://blog.nojaf.com/2015/05/02/testing-a-webcomponent-using-csharp-selenium/
https://github.com/nojaf/SledgehammerSheep

https://justmarkup.com/log/2016/08/indicating-offline/#grey-out-things-not-available-offline

http://stackoverflow.com/questions/30837206/can-open-layer-3-be-used-as-a-web-component
https://github.com/jumpinjackie/ol-polymer
https://github.com/fherdom/idecanarias-ol3
https://github.com/fredj/ol3-polymer
https://github.com/oliverroick/map-components

https://github.com/openlayers/ol3/issues/5435

https://www.smashingmagazine.com/2017/02/designing-html-apis/
https://hacks.mozilla.org/2014/12/mozilla-and-web-components/

https://www.robinwieruch.de/reasons-why-i-moved-from-angular-to-react/

### Tooling

Developer Tooling for Web Components (Google I/O '17)
https://www.youtube.com/watch?v=tKvNeNGmOtU

#### CLI
init
lint
serve
test (requires java ugh#!)
build
analyze

### ASP.NET Core


http://stackoverflow.com/questions/42823951/publishing-polymer-asp-net-core


### Using TypeScript

http://thewebplatformpodcast.com/124-web-components-typescript-and-bears-oh-my
https://github.com/deebloo/ts-custom-elements

https://github.com/nippur72/PolymerTS

https://github.com/Draccoz/twc


DI with typescript
https://github.com/LssPolymerElements/lss-inject

https://github.com/Lodin/polymer2-elements-typings

https://github.com/kaseyhinton/Polymer-Typescript-Starter-Kit


#### Webpack?

https://github.com/tlimpanont/polymer-webpack-typescript


### Calendars


https://www.webcomponents.org/element/PFElements/pf-calendar
https://github.com/PFElements/pf-calendar

jquery based
https://fullcalendar.io/

https://github.com/sorin-davidoi/fullcalendar-calendar

http://stackoverflow.com/questions/35857059/fullcalendar-not-rendering-in-polymer-project

https://github.com/mpachnis/mp-calendar

https://medium.com/@tbeatty/interactive-calendar-element-with-polymer-e2217f43f35c

https://github.com/TimvdLippe/polymer-calendar
https://github.com/Wenqer/paper-calendar

http://www.joeschwartz.com/2015/02/10/a-polymer-event-calendar/
https://github.com/JoppeSchwartz/event-calendar

https://libraries.io/explore/html-polymer-libraries?keywords=calendar&platforms=Bower

### Form Builders


https://github.com/ecoutu/eco-json-schema-form
2.0
https://github.com/DimShadoWWW/eco-json-schema-form

https://polymer-designer.appspot.com/

https://github.com/pmagaz/polymer-dynamic-form
https://www.webcomponents.org/element/PolymerElements/iron-form


https://medium.com/@tbeatty/interactive-calendar-element-with-polymer-c16a6c7e5e37

#### Is this similar to the ract patent issue?
http://polymer.github.io/PATENTS.txt
