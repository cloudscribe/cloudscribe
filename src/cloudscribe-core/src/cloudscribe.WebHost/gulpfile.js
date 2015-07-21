/// <binding Clean='clean' />

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-cssmin"),
  uglify = require("gulp-uglify"),
  project = require("./project.json");

/*
var paths = {
  bower: "./bower_components/",
  lib: "./" + project.webroot + "/js/lib/",
  scripts: "./Scripts/",
  css: "./" + project.webroot + "/css/"
};
*/

var paths = {
    webroot: "./" + project.webroot + "/",
    appJsSrc: "./Scripts/"
};

paths.js = paths.webroot + "js/site/*.js";
paths.minJs = paths.webroot + "js/site/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.appJsDest = paths.webroot + "js/app/";

/*
gulp.task("clean", function (cb) {
  rimraf(paths.lib, cb);
});
*/

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));

    gulp.src(paths.appJsSrc + '**.js')
        .pipe(uglify())
        .pipe(gulp.dest(paths.appJsDest));

});

gulp.task("min:css", function () {
    gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);




/*
gulp.task("minifycss", function () {
    return gulp.src([paths.css + "/*.css",
                     "!" + paths.css + "/*.min.css"])
            .pipe(minifycss())
            .pipe(concat("site.min.css"))
            .pipe(gulp.dest(paths.css));
});
*/
