# animestr
#### A program for scraping anime mrls from various sources
#### NB: The Android (Java) version is deprecated and also does not function
#### This program is currently under heavy development

I am coding this program with speed and efficiency in mind. Regex is forbidden - almost all of the data parsing is done with specific while, for and foreach loops which parse the HTML linearly, so as to increase speed. Although HTML trees are currently used in some places, they will be removed for faster solutions in the future. 

### Features:
- MALParsing
 - [x] Title
 - [x] Alt titles
 - [x] Genres
 - [x] Score
 - [x] Rank
 - [x] Popularity
 - [x] Description
 - [x] Search
