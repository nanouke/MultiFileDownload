const {ipcRenderer} = require('electron');
const download = require('./Download');

$(function(){
    $('#btnLoadFile').click(function(){
        ipcRenderer.send('loadFile');
    });

    ipcRenderer.on('loadFileReply', (event, arg) => {
        let downloads = arg.list;

        console.log(downloads);

        for(let i = 0; i < downloads.length; i++){
            console.log("Test");
            $('#downloadList').prepend(download.objectToDownload(downloads[i]).toHtml());
        }

        

    })

});

