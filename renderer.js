const {ipcRenderer} = require('electron');
const download = require('./Download');
const NotificationSystem = require('./Ui/NotificationSystem');
const Notification = require('./Ui/Notification');

let downloadsList = [];
let simmultaniousDownload = 5;

$(function(){

    let notificationSystem = new NotificationSystem($('#notification-list'));
    notificationSystem.add(new Notification("Notification 01"));
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

        while (!allDownloadsEnd()){

            if(progressCount() < simmultaniousDownload ){

            }

        }

    });

    $(document).on('click','.btn-remove-notification', function(){
        $(this).parent().parent().remove();
    });

});

function allDownloadsEnd() {
    let result = true;
    for(let i = 0; i < downloadsList.length; i++){
       if(downloadsList[i].status != Download.statusEnum.DONE || Download.statusEnum.ERROR){
           result = false;
       }
    }
    return result;
}

function progressCount() {
    let result = 0;

    for(let i = 0; i < downloadsList.length; i++){
        if(downloadsList[i].status == Download.statusEnum.PROGRESS){
            result++;
        }
    }
    return result;
}

