const {ipcRenderer} = require('electron');
const download = require('./Download');

let downloadsList = [];

$(function(){

    let $downloadList = $('#downloadList');

    $('#btnLoadFile').click(function(){
        $(this).blur();
        ipcRenderer.send('loadFile');
    });

    ipcRenderer.on('refreshDownload', (event, arg) => {
        let downloads = arg.list;
        $downloadList.empty();

        for(let i = 0; i < downloads.length; i++){
            let d =  download.objectToDownload(downloads[i]);
            downloads.push = d;

            $downloadList.prepend(d.toHtml());
        }
    });


    $(document).on('click', '#downloadList tr', function(e){
        $(this).toggleClass('selected');

    });

    $(document).on('click', '#downloadList .btn-remove-item', function(e){
        e.stopPropagation();

    });

    $('#btnStartDownload').click(function(){

    });

});

