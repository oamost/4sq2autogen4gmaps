# samples

## generated KML output sample:

![image](https://github.com/user-attachments/assets/2107ef94-7a41-44be-a690-ba682fb00103)

## usage sample on Google My Maps:

![image](https://github.com/user-attachments/assets/5f9aea1b-7eee-4d4d-bbc4-c169d38df148)

# how-to use

1. go to https://foursquare.com/developers/home
2. create a new project
3. copy `Client Id` and `Client Secret` into a notepad for later use - can be found under the newly created project's Settings.
4. don't forget to fill redirect URL, which most be `https://www.example.com`
5. clone this repo
6. open `./src/Authentication.html` and authorize your newly created app with your Foursquare account
7. you will be redirected to `example.com/code=COPY_THIS_PART` <- this will expire after a few minutes, recommended to generate it after step #9
8. `$ dotnet build`
9. `$ ./bin/Debug/net8.0/4sq2autogen4gmaps`
10. paste generated `Code` into the console app
11. paste `Client Id` into the console app
12. paste `Client Secret` into the console app
13. wait for the generation to be finished
14. open https://www.google.com/maps/d/u/0/?hl=en and create your new map
15. click on "New Layer", then click on `Import` and browse the `.kml` file located in the repo
16. the rest is straightforward - you can set a few map styles and you can set the venue labels to be shown on Google My Maps
