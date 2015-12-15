import java.util.ArrayList;


public class BowlingScorer {
	
	private static final int STRIKE_VALUE = 10;
	private static final String SPARE = "SP";
	private static final String STRIKE = "ST";
	private static final String NORMAL = "N";
	private static final int PERFECT_GAME = 12;
	private static final int MAXIMUM_ROLLS = 21;
	
	//VALID: Score sheet is accurate and complete:
	private static Integer[] player1ScoreSheet = {4, 4, 10, 5, 4, 4, 6, 3, 7, 4, 6, 10, 8, 2, 1, 0, 10, 9, 1};
	
	//VALID: Score sheet is accurate and complete:
	private static Integer[] player2ScoreSheet = {3, 7, 10, 10, 5, 2, 6, 3, 10, 4, 5, 7, 2, 4, 5, 3, 4};
	
	//VALID: PERFECT GAME!  Score of 300!:
	private static Integer[] player3ScoreSheet = {10,10,10,10,10,10,10,10,10,10,10,10};
	
	//INVALID: Score sheet is incomplete, with only nine frames:
	private static Integer[] player4ScoreSheet = {10,10,6,4,8,1,9,0,4,5,10,3,7,2,0};
		
	//INVALID: Score sheet is too long... validation logic will reject it:
	private static Integer[] player5ScoreSheet = {4, 4, 10, 5, 4, 4, 6, 3, 7, 4, 6, 10, 8, 2, 1, 0, 10, 9, 1, 10, 4, 5, 2, 3, 5, 3, 10};
	
	//INVALID: Some Scores are too high.  A single ball cannot attain greater than ten points:
	private static Integer[] player6ScoreSheet = {3, 7, 10, 10, 5, 2, 6, 3, 10, 12, 15, 37, 2, 4, 5, 3, 4};
	
	//INVALID: Some Frames are too high.  A frame of two balls cannot likewise not be greater than ten points:
	private static Integer[] player7ScoreSheet = {3, 7, 10, 10, 5, 6, 6, 3, 10, 7, 5, 7, 2, 4, 5, 3, 4};
	
	
	public static void main(String[] args) {
		
		displayInstructionsMarquee();
		
		System.out.println ("\nPROCESSING PLAYER SCORE SHEET...");
		
		// READ IN SCORE-SHEET (Choose the one you want): 
		Integer[] playerSheet = player3ScoreSheet;
		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		
		//Initial Validation (move to private method):
		if (playerSheet.length < PERFECT_GAME) {
			System.out.println(" *** The tally sheet is incomplete.  At least 12 rolls are required.  That would be a perfect 300 game.");
		} else if (playerSheet.length > MAXIMUM_ROLLS) {
			System.out.println(" *** The tally sheet contains too many rolls.");
		}
		
		//Convert Score Sheet into Frames (track both totals and special frame types separately,  The frame type would be a Strike or a Spare):
		ArrayList<Integer> frameTotals = new ArrayList<Integer>();
		ArrayList<String> frameType = new ArrayList<String>();
		
		Integer currentFrame = 1;		
		int frameRoll = 1;
		Integer currentFrameTotal = 0;
		boolean errors = false;
		
		try {
			
			for (int i = 0; i < playerSheet.length; i++) {
				
				if (currentFrame > 10) {
					break; //End iteration... all ten frames are processed
				}
				
				Integer currentRoll = playerSheet[i];
				if (currentRoll > 10) {
					System.out.println(" *** Error: A single ball cannot knock down more than ten pins. Roll of " + currentRoll + " is impossible.");
					errors = true;
				}
			
				// Add total points for the frame:
				if (frameRoll == 1 && currentRoll == STRIKE_VALUE ) {
					currentFrameTotal = currentRoll + playerSheet[i+1] + playerSheet[i+2];
					frameTotals.add(currentFrameTotal);
					frameType.add(STRIKE);
					
					//Reset next frame:
					currentFrameTotal = 0;
					currentFrame++;
					frameRoll = 1;  //Reset frame roll back to one.
					
				} else if (frameRoll == 2 && currentRoll == STRIKE_VALUE ) {
					//Logic should prevent this state from occurring:
					System.out.println("Invalid entry... a STRIKE cannot ocur on the second roll of any frame.");
					break;
					
				} else {
					if (frameRoll == 2) {
						currentFrameTotal += currentRoll;
						if (currentFrameTotal > 10) {
							System.out.println(" *** Error: A frame of two rolls cannot knock over more than 10 pins... " + currentFrameTotal + " is not possible.");
							errors = true;
						}
						
						frameType.add(currentFrameTotal == STRIKE_VALUE ? SPARE : NORMAL);
						
						//Calculate Frame Total (a spare or no spare):
						if (currentFrameTotal == STRIKE_VALUE) {
							currentFrameTotal += playerSheet[i+1];
							frameTotals.add(currentFrameTotal);	
						} else {
							frameTotals.add(currentFrameTotal);
						}					
						
						//Reset next frame:
						currentFrameTotal = 0;
						currentFrame++;
						frameRoll = 1;  //Reset frame roll back to one.
						
					} else {
						//Role 1 of current Frame ~ no Strike or Spare:
						currentFrameTotal += currentRoll;	
						frameRoll = 2;
					}
					
				}				
				
				//Special logic for frame 10 - up to three rolls
							
			}
			
			if (frameTotals.size() < 10) {
				System.out.println (" *** Error: The score sheet is incomplete.  Ten frames are required. Only " + frameTotals.size() + " were entered.");
				errors = true;
			} else if (frameTotals.size() > 10) {
				System.out.println (" *** Error: The score sheet has too many entries.  Only ten frames are allowed.  Counted " + frameTotals.size());
			} else if (!errors) {
				System.out.println (" *** Success... Score sheet is complete.  All frames have been calculated.");
				System.out.println (" *** Rendering player scores.");
			} else {
				System.out.println (" *** An error occurred while processing the score sheet... terminating.");
			}
			
			
			// Process Player Score, taking into account multi-framed scoring:
			// Spit out results (ideally with roll values and scoring side by side, like an actual bowling score sheet)
		
			if (!errors) {
				System.out.println("\n");
				System.out.println("\n                  P L A Y E R   S C O R E   S H E E T \n");
				System.out.println("╔═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═╦═══════╗");		
				System.out.print("║");
				
				try {
					int frameCount = 0;
					for (int i = 0; i < playerSheet.length - 2; i+=2) {				
						String frame = frameType.get(frameCount);
						switch (frame) {
							case STRIKE:
								System.out.print("   │X│");
								i--; //Cheat the iteration for Strikes, since we have only one ball...
								break;
							case SPARE:
								System.out.print(" " + playerSheet[i] + " │/│");
								break;
							case NORMAL:
								System.out.print(" " + playerSheet[i] + " │" + playerSheet[i+1] + "│");
								break;
						}
						frameCount++;
					}
				} catch (IndexOutOfBoundsException iobe) {
					System.out.println (" *** The score sheet has exceeded the alotted length... ending score calculation.");
				}
				
				System.out.println(" ║ TOTAL ║");		
				System.out.println("║   └─┤   └─┤   └─┤   └─┤   └─┤   └─┤   └─┤   └─┤   └─┤   └─┴─╢ SCORE ║");
				
				System.out.print("║ ");
				Integer playerTotal = 0;
				String spacer = "    ";
				for (int frame = 0; frame < frameTotals.size(); frame++ ) {
					playerTotal += frameTotals.get(frame);
					String frameSpace = spacer.substring(0, 4 - playerTotal.toString().length());
					System.out.print(playerTotal + frameSpace + "│ ");
					//System.out.format("%3s", "");
		
		//			System.out.print();
				}
				System.out.print("║ ───── ║\n");
				String padding = spacer.substring(0, 4 - playerTotal.toString().length());
				System.out.println("║     │     │     │     │     │     │     │     │     │       ║  " + playerTotal + padding + " ║");
				System.out.println("╚═════╧═════╧═════╧═════╧═════╧═════╧═════╧═════╧═════╧═══════╩═══════╝");
	
			} else {
				System.out.println(" *** Terminating program.");
			}
		
		} catch (Exception e) {
			System.out.println("*** An Exception Error has occurred... program is terminated.");
			System.out.println("    Here is the Output:");
			e.printStackTrace();
		} 
		
	}

	
	private static void displayInstructionsMarquee() {
		String headerFill = "\n\t\t ║                                              ║";
		String headerBorder = "\n\t\t ╫──────────────────────────────────────────────╫";
		System.out.print( headerBorder + headerFill );
		System.out.print( "\n\t\t ║   STG ~ Code Challenge #4 ~ Bowling Scorer   ║" );
		System.out.print( "\n\t\t ║   December 2015 ~ Quarter Four Challenges    ║" );
		System.out.print( "\n\t\t ║   Michael Monson ~ Console Implmentation     ║" );
		System.out.print( headerFill + headerBorder );

		System.out.println( "\n\nThe local bowling alley was complaining this week of bowlers who were manually entering in invalid scores in order to pad their numbers--one person even bowled a 350! They’ve asked us to come up with a simple, yet failproof way to score their bowling games..." );
		System.out.println( "Given an array of 10 bowling frames for each player, compute each player’s final bowling score." );
		System.out.println( "\nSample scoresheets have already been entered for your sanity.  A number of scenarios have been implemented, including invalid scoresheets.  Simply update the code (variable: playerSheet) to read in the desired scoresheet for processing. \nOr feel free to enter your own array of scores.");
		
	}
	
}
