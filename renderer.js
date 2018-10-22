const {ipcRenderer} = require('electron')

$(function(){
    $('#btnLoadFile').click(function(){
        ipcRenderer.send('loadFile');
    });

    ipcRenderer.on('loadFileReply', (event, arg) => {
        console.log(arg);
    })

});

