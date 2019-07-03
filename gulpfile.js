/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

const gulp = require('gulp');
const npmFiles = require('npmfiles');

// var plugins = require('gulp-load-plugins')();
// var bowerFiles = require('main-bower-files');
// var filter = require('gulp-filter');

const paths = {
    lib: './wwwroot/lib/'
};

gulp.task('build-lib', function () {
    return gulp.src(npmFiles())
        .pipe(gulp.dest(paths.lib));
});

// default task builds for prod
gulp.task('default', gulp.series('build-lib', function(done) {    
    done();
}));
