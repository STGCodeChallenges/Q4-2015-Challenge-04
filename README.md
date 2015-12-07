#Challenge #4 (due 12/14/2015): Bowling Scorer
The local bowling alley was complaining this week of bowlers who were manually entering in invalid scores in order to pad their numbers--one person even bowled a 350!  They’ve asked us to come up with a simple, yet failproof way to score their bowling games.

##Description
Given an array of 10 bowling frames for each player, compute each player’s final bowling score.  Here are the requirements:
* As implied in the introduction, the scoring algorithm must be failproof; this means you should plan for just about any bad input value and provide heavy validation to prevent any form of cheating.  It’s up to you how to best handle invalid data.
* A frame consists of one or two balls, except for frame ten, which will be two or three balls
* If you knock down less than ten pins in a frame, the score for that frame is the sum of the two balls
* If you knock down ten pins after two balls (a spare), your score is the sum of the two balls plus the number of pins knocked down in the next ball
* If you knock down ten pins after just one ball (a strike), your total score for that frame is ten plus the number of pins knocked down in the next two balls
* The above rules work a bit differently for the 10th frame.  In that frame, if one bowls a spare or a strike, they are guaranteed an additional third ball (i.e. potential for three strikes on the 10th frame); the total score for that frame is simply the total number of pins knocked down from the two or three balls.  In other words, a game of all strikes (12 total) yields a perfect score of 300.
* It’s up to you how to represent a strike or spare in the array of frames -just be clear and consistent.

##Bonus Entry
The possibilities for unit tests (both for the scoring algorithm and for validation) are endless!  Write up at least ten solid unit tests to ensure a decent amount of code coverage.  Which testing framework you use is up to you; at the very minimum, you can just write some test functions that exercise your program.

##Submission Instructions

GitHub repo: https://github.com/STGCodeChallenges/Q4-2015-Challenge-04

Google Form (to be filled out upon completion of challenge): https://docs.google.com/a/stgconsulting.com/forms/d/17nMJYu4Sq3_oOe5GtiTRyy2uxeMg3FnePL09Zr6U02I/viewform?usp=send_form

