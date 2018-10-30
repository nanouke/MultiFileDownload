const crypto = require('crypto');
const hash = crypto.createHash('md5');

class Notification{
    constructor(title, description = null, level = Notification.Level.NORMAL , action = null){
        this.title = title;
        this.description = description;
        this.level = level;
        this.action = action;

        hash.update(this.title);
        this.id = hash.digest('hex');

    }


    render(){
        let render = "";
        render += '<div class="notification" id="' + this.id + '">';
        render += '<div class="left">';
        render += '<h3>' + this.title + '</h3>';
        if(this.description != null){
            render += '<p>' + this.description + '</p>';
        }
        render += '</div>';
        render += '<div class="right">';
        render += '<button class="btn btn-none text-white btn-remove-notification" data-id="' + this.id + '"><i class="fas fa-times"></i></button>'
        render += '</div>';
        render += '</div>';

        return render;
    }

}

Notification.Level = {'LOW': 3, 'NORMAL': 2, 'IMPORTANT' : 1};

module.exports = Notification;