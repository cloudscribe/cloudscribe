/// <binding Clean='clean' />

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  minifycss = require("gulp-minify-css"),
  concat = require("gulp-concat"),
  fs = require("fs");

eval("var project = " + fs.readFileSync("./project.json"));

var paths = {
  bower: "./bower_components/",
  lib: "./" + project.webroot + "/js/lib/",
  scripts: "./Scripts/",
  css: "./" + project.webroot + "/css/"
};

gulp.task("clean", function (cb) {
  rimraf(paths.lib, cb);
});

gulp.task("copy", ["clean"], function () {
  var bower = {
    "bootstrap": "bootstrap/dist/**/*.{js,map,css,ttf,svg,woff,eot}",
    "bootstrap-touch-carousel": "bootstrap-touch-carousel/dist/**/*.{js,css}",
    "hammer.js": "hammer.js/hammer*.{js,map}",
    "jquery": "jquery/jquery*.{js,map}",
    "jquery-ajax-unobtrusive": "jquery-ajax-unobtrusive/*.{js,map}",
    "jquery-validation": "jquery-validation/jquery.validate.js",
    "jquery-validation-unobtrusive": "jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
    "jquery-ui": "jquery-ui/**/*.{js,map,css}"
  }

  for (var destinationDir in bower) {
    gulp.src(paths.bower + bower[destinationDir])
      .pipe(gulp.dest(paths.lib + destinationDir));
  }

  gulp.src(paths.scripts + '**.js')
        .pipe(gulp.dest(paths.lib));

});

gulp.task("minifycss", function () {
    return gulp.src([paths.css + "/*.css",
                     "!" + paths.css + "/*.min.css"])
            .pipe(minifycss())
            .pipe(concat("site.min.css"))
            .pipe(gulp.dest(paths.css));
});
