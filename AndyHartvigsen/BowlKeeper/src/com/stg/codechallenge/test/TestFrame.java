package com.stg.codechallenge.test;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;

import org.junit.Test;

import com.stg.codechallenge.BowlingException;
import com.stg.codechallenge.Frame;

public class TestFrame {

	@Test
	public void testTenthFrame() {
		Frame frame = new Frame(10);
		
		try {
			frame.addBall(10);
			frame.addBall(10);
			frame.addBall(10);
		} catch (BowlingException e) {
			fail("This should be a valid score for the tenth frame.");
		}
		
		assertTrue(frame.getScore() == 30);
	}

	@Test(expected=BowlingException.class)
	public void testElevenPinsOneBall() throws BowlingException {
		Frame frame = new Frame(1);
		frame.addBall(11);		
	}
	
	@Test(expected=BowlingException.class)
	public void testElevenPinsTwoBalls() throws BowlingException {
		Frame frame = new Frame(1);
		frame.addBall(5);
		frame.addBall(6);		
	}
	
	
	@Test
	public void testSpare() {
		Frame frame = new Frame(1);
		try {
			frame.addBall(4);
			frame.addBall(6);
		} catch (BowlingException e) {
			fail("This should result in a spare.");
		}
		assertTrue(frame.isSpare());
	}
	
	@Test
	public void testStrike() {
		Frame frame = new Frame(1);
		try {
			frame.addBall(10);
		} catch (BowlingException e) {
			fail("This should result in a strike.");
		}
		assertTrue(frame.isStrike());
	}
}
