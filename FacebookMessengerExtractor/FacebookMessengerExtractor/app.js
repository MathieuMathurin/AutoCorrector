//Le accessToken doit etre pris a partir de https://developers.facebook.com/tools/explorer car mon app FB ne peut pas utiliser d'API inferieur a 2.5
var accessToken = 'CAACEdEose0cBACt2J31UzTj9Fdb52UHQ4rhQhlfonV01MNzUTOZA4M1TgWHkRarYfCcFQcrAhcLKoVCwL4fuWo7ZA4qx27ZCIsDt8TZAPjLlhmoGaBa8TZCZBioBn9dcQM8blL8NQesMB26jsLngJMiI4JJmOhkWwgQlWxyeo3ZALNvwPhWD3zcGePWzGehkRJ8ZCRwBvhshIQZDZD',
    _ = require('lodash'),
    request = require('request-promise');

var conversationsId = [];
var isConversationsFetchComplete = false;
var isConversationsMessagesComplete = false;

var fetchConversationsId = function(url){
    var req = request(url)
        .then(function (body) {
            var jBody = JSON.parse(body);
        
            var data = jBody.data;
            //console.log(jBody.paging);
            if (jBody.paging && jBody.paging.next) {
                fetchConversationsId(jBody.paging.next);
            }
            else{
                isDone = true;
            }
            var tempConv = _.map(data, function (d) { return d.id });
            conversationsId = conversationsId.concat(tempConv);
        })
        .finally(function () {            
            if (isConversationsFetchComplete) {
                fetchAndSaveConversationMessage();
            }
        });
}

fetchConversationsMessages() {
    conversationsId.forEach(function (id) {
        fetchConversationMessages(id);
    });
    //code pour ecrire dans le fichier. J'envisage aussi de le faire faire a chaque conversation dans un append
}

var fetchConversationMessages = function (id) {
    var req = request('https://graph.facebook.com/v2.3/' + id + '?access_token=' + accessToken)
    .then(function (body){
        var jBody = JSON.parse(body);
        
        var data = jBody.comments.data;
        //console.log(data);
        if (jBody.paging && jBody.paging.next) {
            //code pour caller le next d'une conversation
        }
        var temp = _.chain(data).filter(function (d) { return d.from.name == 'Mathieu Mathurin' }).map(function (m) { return m.message }).value();
        //console.log(temp);
    })
    .finally(function () {
        
    })        
};

//fetchConversationsId('https://graph.facebook.com/v2.3/me/inbox?access_token=' + accessToken + '&debug=all&fields=id&format=json&method=get&pretty=0&suppress_http_code=1');
fetchConversationMessages(234222416732567);