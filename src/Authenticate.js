// helper to be redirected to example.com with the code for the access token (oauth)
//
function authenticate()
{
    var client_id = document.getElementById("foursquare_client_id");

    if (!client_id.value)
    {
        alert("Client ID is missing!");   
        return;
    }

    var redirect_uri = "https://www.example.com";

    var auth_uri =   "https://foursquare.com/oauth2/authenticate" +
                    "?client_id=" + client_id.value +  
                    "&response_type=code" +
                    "&redirect_uri=" + redirect_uri;

    window.location.href = auth_uri;
}