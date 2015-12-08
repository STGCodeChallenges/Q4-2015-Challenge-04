import java.util.ArrayList;


public class BowlingScorer {
	
	private static final int STRIKE_VALUE = 10;
	private static final String SPARE = "SP";
	private static final String STRIKE = "ST";
	private static final String NORMAL = "N";
	private static final int PERFECT_GAME = 12;
	private static final int MAXIMUM_ROLLS = 21;
	private static Integer[] player1ScoreSheet = {4, 6, 10, 4, 0, 9,1};
	
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
		
		
		for( int i = 0; i < playerSheet.length - 1; i++) {
			
			Integer currentFrameTotal = 0;
			int frameRoll = 1;
			
			if (currentFrame > 10) {
				break;
			}
			
			if (frameRoll == 1 && playerSheet[i] == STRIKE_VALUE ) {
//				playerTotal += playerSheet[i];
				frameTotals.add(STRIKE_VALUE);
				frameType.add(STRIKE);
				break;
				
			} else if (frameRoll == 2 && playerSheet[i] == STRIKE_VALUE ) {
				//Logic should prevent this state from occurring:
				System.out.println("Invalid entry... a STRIKE cannot ocur on the second roll of any frame.");
				break;
				
			} else {
				if (frameRoll == 2) {
					currentFrame++;
					frameRoll = 1;
					frameTotals[currentFrame] = currentFrameTotal;
					currentFrameTotal = 0;
				}
				
				currentFrameTotal += playerSheet[i];
				
			}
			
			
			//Special for frame 10 - up to three rolls
			
			currentFrame++;
		}
		
		// Process Player Score:
		
		int playerTotal = 0;
		
		
		
		
		// spit out results

	}

}
