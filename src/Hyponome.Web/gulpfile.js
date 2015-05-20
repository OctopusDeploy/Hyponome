/// <binding Clean='clean' />

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  fs = require("fs"),
  project = require("./project.json");

var paths = {
  bower: "./bower_components/",
  npm: "./node_modules/",
  lib: "./" + project.webroot + "/lib/"
};

gulp.task("clean", function (cb) {
  rimraf(paths.lib, cb);
});

gulp.task("copy", ["clean"], function () {
  var bower = {
    "bootstrap": "bootstrap/dist/**/*.{js,map,css,ttf,svg,woff,eot}",
    "jquery": "jquery/dist/jquery*.{js,map}",
    "ace": "ace-builds/src-noconflict/**/*.{js,map,css}",
    "fontawesome": "fontawesome/**/*.{css,map,ttf,svg,woff,eot}",
    "momentjs": "momentjs/**/moment*.js"
  };

  for (var destinationDir in bower) {
    gulp.src(paths.bower + bower[destinationDir])
      .pipe(gulp.dest(paths.lib + destinationDir));
  }
});
