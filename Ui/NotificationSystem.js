class NotificationSytem{

    constructor(notificationListSelector, notificationButton){
        this.notificationListSelector = notificationListSelector;
        this.notificationButton = notificationButton;    
    }


    add(notification) {
        this.notificationListSelector.append(notification.render());
    }

    remove(id){
        this.notificationListSelector.getElementById(id).removeChild();
    }


}

module.exports = NotificationSytem;