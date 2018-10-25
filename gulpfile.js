'use strict';

let gulp = require('gulp');
let sass = require('gulp-sass');

gulp.task('sass', function () {
    return gulp.src('./sass/app.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./css'));
});

gulp.task('sass:watch', function () {
    gulp.watch('./sass/*.scss', ['sass']);
});
