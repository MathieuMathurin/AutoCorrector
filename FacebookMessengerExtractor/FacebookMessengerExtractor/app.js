//Regarde plus bas pour les instruction de fonctionnement

//Configs
var user = 'Mathieu Mathurin'
//Le accessToken doit etre pris a partir de https://developers.facebook.com/tools/explorer car mon app FB ne peut pas utiliser d'API inferieur a 2.5
var accessToken = 'CAACEdEose0cBADXgY2mnKLAUbgVCcnc12FLdZAOO8gOIX3iNSwlsV8XLFfUZC5KTvu7zQi2zxSitaAT6Ff2t6UmM95gbACdGdon8ZCvZCtDKPBsHjWCyAnTKnLK8J3CZB3DCrAMGhAI84hhf0xnabUw6K4EVr5ZAp6tJZCOZBUcqfSZC8fQQ3FNhdF7Xbk3i6Y5DFlbtTuotrFAZDZD';
var conversation = 251195684931060;

var _ = require('lodash'),
    request = require('request-promise'),
    fs = require('fs');

var conversationsId = [];
var conversationsMessages = [];
var isConversationsIdFetchComplete = false;
var conversationsFetchStatus = {};

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
                isConversationsIdFetchComplete = true;
            }
            var tempConv = _.map(data, function (d) { return d.id });
            conversationsId = conversationsId.concat(tempConv);
        })
        .finally(function () {
        if (isConversationsIdFetchComplete) {
                writeConversationIds();
                //Cette ligne servirait si on avait pas de limite de call
                //fetchAndSaveConversationsMessages();                              
            }
        });
}


//Sert a ecrire les ids dans un fichier quand tous les call sont fait (pas la meilleur facon puisque le code est rendu sync mais tentait pas de le changer
var writeConversationIds = function (){
    var ids = _.join(conversationsId, '\n');
    fs.writeFile('conversationsIds.txt', ids, function (err) {
        if (!err) {
            console.log('Done');
        }
    });
}

//Construit l'url pour la premiere page puis lance la recursion
var fetchAndSaveConversationsMessages = function(id) {
    var url = 'https://graph.facebook.com/v2.3/' + id + '/comments?access_token=' + accessToken;
    fetchConversationMessages(url, id);
}

//Va chercher les message du user et les enregistre
var fetchConversationMessages = function (url, id) {
    var req = request(url)
    .then(function (body) {
        var jBody = JSON.parse(body);        
        var data = jBody.data;
        //console.log(data);        
        if (jBody.paging && jBody.paging.next) {
            //code pour caller le next d'une conversation
            fetchConversationMessages(jBody.paging.next, id);
        }
        var temp = _.chain(data).filter(function (d) { return d.from.name == user }).map(function (m) { return m.message }).value();
        conversationsMessages = conversationsMessages.concat(temp);
        temp = _.join(temp, '\n');
        fs.appendFile('messages-' + id  + '.txt', temp, 'utf8', function (err) {
            if (!err) {
                console.log('Writing to file');
            }
        });
    });        
};

/* Pour aller cherche tout t'es conversation ID, uncomment la ligne suivante et comment l'autre.
 * Une fois fait, tu auras tous les ids de tes conversation dans le fichier conversationIds.txt
 * 
 * Le truc c'est de partir manuellement la 2e ligne en indiquant le code de conversation. Si c'est des grosses conversations ca va buster car il y a une limite de 200 call / heure.
 * Tu peux aussi utiliser le lien du access key pour cibler de quelle conversation il s'agit. (Par confidentialite ou simplement parce que tu ne parle pas beaucoup dans cette conversation)
 * */

//fetchConversationsId('https://graph.facebook.com/v2.3/me/inbox?access_token=' + accessToken + '&debug=all&fields=id&format=json&method=get&pretty=0&suppress_http_code=1');
fetchAndSaveConversationsMessages(conversation);