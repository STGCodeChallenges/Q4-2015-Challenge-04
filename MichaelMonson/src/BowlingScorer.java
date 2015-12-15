import java.util.ArrayList;


public class BowlingScorer {
	
	private static final int STRIKE_VALUE = 10;
	private static final String SPARE = "SP";
	private static final String STRIKE = "ST";
	private static final String NORMAL = "N";
	private static final int PERFECT_GAME = 12;
	private static final int MAXIMUM_ROLLS = 21;
	
	//Score sheet is accurate and complete:
	private static Integer[] player1ScoreSheet = {4, 4, 10, 5, 4, 4, 6, 3, 7, 4, 6, 10, 8, 2, 1, 0, 10, 9, 1};
	
	//Score sheet is incomplete, with only nine frames:
	private static Integer[] player2ScoreSheet = {10,10,6,4,8,1,9,0,4,5,10,3,7,2,0};
	
	public static void main(String[] args) {
		
		// Read in ScoreSheet Array (Choose the one you want)
		Integer[] playerSheet = player1ScoreSheet;
		
		//Initial Validation (move to private method):
		if (playerSheet.length < PERFECT_GAME) {
			System.out.println("The tally sheet is incomplete.  At least 12 rolls are required.  That would be a perfect 300 game.");
		} else if (playerSheet.length > MAXIMUM_ROLLS) {
			System.out.println("The tally sheet contains too many rolls.");
		}
		
		//Convert Score Sheet into Frames (track both totals and special frame types separately,  The frame type would be a Strike or a Spare):
		ArrayList<Integer> frameTotals = new ArrayList<Integer>();
		ArrayList<String> frameType = new ArrayList<String>();
		
		Integer currentFrame = 1;		
		int frameRoll = 1;
		Integer currentFrameTotal = 0;
		
		for (int i = 0; i < playerSheet.length; i++) {
			
			if (currentFrame > 10) {
				break; //End iteration... all ten frames are processed
			}
		
			// Add total points for the frame:
			if (frameRoll == 1 && playerSheet[i] == STRIKE_VALUE ) {
				currentFrameTotal = playerSheet[i] + playerSheet[i+1] + playerSheet[i+2];
				frameTotals.add(currentFrameTotal);
				frameType.add(STRIKE);
				System.out.println("Roll #" + i + " : Strike added.  Points = " + currentFrameTotal);
				
				//Reset next frame:
				currentFrameTotal = 0;
				currentFrame++;
				frameRoll = 1;  //Reset frame roll back to one.
				
			} else if (frameRoll == 2 && playerSheet[i] == STRIKE_VALUE ) {
				//Logic should prevent this state from occurring:
				System.out.println("Invalid entry... a STRIKE cannot ocur on the second roll of any frame.");
				break;
				
			} else {
				if (frameRoll == 2) {
					currentFrameTotal += playerSheet[i];					
					frameType.add(currentFrameTotal == STRIKE_VALUE ? SPARE : NORMAL);
					
					//Calculate Frame Total (a spare or no spare):
					if (currentFrameTotal == STRIKE_VALUE) {
						currentFrameTotal += playerSheet[i+1];
						frameTotals.add(currentFrameTotal);	
						System.out.println("Roll #" + i + " : Spare added.  Points = " + currentFrameTotal);
					} else {
						frameTotals.add(currentFrameTotal);
						System.out.println("Roll #" + i + " : Second Frame added (No Spare or Strike).  Points = " + currentFrameTotal);
					}					
					
					//Reset next frame:
					currentFrameTotal = 0;
					currentFrame++;
					frameRoll = 1;  //Reset frame roll back to one.
					
				} else {
					//Role 1 of current Frame ~ no Strike or Spare:
					currentFrameTotal += playerSheet[i];	
					frameRoll = 2;
				}
				
			}
			
			
			//Special logic for frame 10 - up to three rolls
						
		}
		
		
		// Process Player Score, taking into account multi-framed scoring:
		// Spit out results (ideally with roll values and scoring side by side, like an actual bowling score sheet)
		System.out.println("\n");
		System.out.println("\n                  P L A Y E R   S C O R E   S H E E T \n");
		System.out.println("╔═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═══╤═╤═╦═══════╗");		
		System.out.print("║");
		
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
		

	}

}
