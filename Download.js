const http = require('http');
const fs = require('fs');

class Download{

    constructor(url, fileName, path = null, status = Download.statusEnum.WAIT, progress = 0.0, speed = 0){
        this.url = url;
        this.fileName = fileName;
        this.path = path;
        this.status = status;
        this.progress = progress;
        this.speed = speed;

    }

    static objectToDownload(obj){
        return new Download(obj.url, obj.fileName, obj.path, obj.status, obj.progress, obj.speed);
    }


    startDownload(){
        return new Promise(function(resolve, reject){

            let req = request({
                method: 'GET',
                uri: this.url
            });

            req.pipe(this.path);

            req.on('response', function(data){

                console.log(data.header['content-lenght']);

            });

            req.on('error', function(e){
                console.log("Error :" + e);

                reject();
            });

            req.on('end', function(){


                resolve();
            })

        });
    }


    toHtml(){
        let html = '<tr>';
        html += '<td>' + this.fileName + '</td>';
        html += '<td>' + this.url + '</td>';
        html += '<td>' + this.progress + '</td>';
        html += '<td>' + this.speed + '</td>';
        html += '<td>' + this.status + '</td>';
        html += '<td><a class="btn-remove-item" data-url="' + this.url + '" title="Remove"><i class="fas fa-times"></i></a></td>'
        html += '</tr>';

        return html;
    }


};
Download.statusEnum = {WAIT: 'wait', PROGRESS: 'progress', DONE: 'done', ERROR: 'error' };


module.exports = Download;