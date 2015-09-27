/// <binding AfterBuild='min:js' Clean='clean' />

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-cssmin"),
  uglify = require("gulp-uglify"),
  project = require("./project.json");


var paths = {
    webroot: "./" + project.webroot + "/"
};

paths.devJs = paths.webroot + "devjs/**/*.js";
//paths.js = paths.webroot + "js/*.js";
//paths.minJs = paths.webroot + "js/*.min.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
//paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.lib = paths.webroot + "lib/";
paths.productionJs = paths.webroot + "js/";


gulp.task("clean:js", function (cb) {
    //rimraf(paths.concatJsDest, cb);
    //rimraf(paths.productionJs, cb);

    rimraf(paths.productionJs + "ckeditor/", function (err) {
        if (err) { throw err; }
        // done
    });

    rimraf(paths.productionJs + "metisMenu/", function (err) {
        if (err) { throw err; }
        // done
    });

    rimraf(paths.productionJs, function (err) {
        if (err) { throw err; }
        // done
    });


});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    //gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        //.pipe(concat(paths.concatJsDest))
     //   .pipe(uglify())
     //   .pipe(gulp.dest("."));
    /*
    gulp.src(paths.appJsSrc + '**.js')
        .pipe(uglify())
        .pipe(gulp.dest(paths.appJsDest));
        */
    gulp.src(paths.devJs)
        .pipe(uglify())
        .pipe(gulp.dest(paths.productionJs));


});

gulp.task("copy:jslibs", function (cb) {
    gulp.src(paths.lib + "ckeditor/**")
        //.pipe(uglify())
        .pipe(gulp.dest(paths.productionJs + "ckeditor"));

    gulp.src(paths.lib + "metisMenu/**")
        //.pipe(uglify())
        .pipe(gulp.dest(paths.productionJs + "metisMenu"));

    // remove some files we don't want
    // probably there is a better way to only copy the ones we want
    /*
    rimraf(paths.productionJs + "ckeditor/samples/", function (err) {
        if (err) { throw err; }
       
    });
    */
    /*
    rimraf(paths.productionJs + "ckeditor/plugins/scayt/dialogs/", function (err) {
        if (err) { throw err; }

    });

    rimraf(paths.productionJs + "ckeditor/plugins/scayt/", function (err) {
        if (err) { throw err; }
        
    });

    rimraf(paths.productionJs + "ckeditor/plugins/wsc/dialogs/", function (err) {
        if (err) { throw err; }

    });

    rimraf(paths.productionJs + "ckeditor/plugins/wsc/", function (err) {
        if (err) { throw err; }
        
    });

    */

    

});

gulp.task("min:css", function () {
    gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);

