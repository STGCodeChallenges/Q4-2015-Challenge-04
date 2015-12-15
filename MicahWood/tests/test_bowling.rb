require 'minitest/autorun'
require 'app/bowling'

describe Bowling do
  before do
    @game = Bowling.new
  end

  it "can score a game of all gutter balls" do
    (1..20).each { @game.roll(0) }
    @game.score.must_equal 0
  end

  it "can score the sum of all knocked down pins" do
    (1..20).each { @game.roll(1) }
    @game.score.must_equal 20
  end

  it "awards a one roll bonus for every spare" do
    @game.roll(2)
    @game.roll(8) # we got a spare
    @game.roll(5)

    (1..17).each { @game.roll(0) }
    @game.score.must_equal 20
  end

  it "awards a two roll bonus for every strike" do
    @game.roll(10)
    @game.roll(7)
    @game.roll(2)

    (1..17).each { @game.roll(0) }
    @game.score.must_equal 28
  end

  it "scores a perfect game" do
    (1..12).each { @game.roll(10) }
    @game.score.must_equal 300
  end

  it "follows Walter Sobchak rules for cheating" do
    proc { @game.roll(-10) }.must_output "MARK IT ZERO!"
    proc { @game.roll(20) }.must_output "MARK IT ZERO!"
    proc { @game.roll('A') }.must_output "MARK IT ZERO!"
    proc { @game.roll([]) }.must_output "MARK IT ZERO!"
    proc { @game.roll(Object.new) }.must_output "MARK IT ZERO!"
  end

  it "should follow through with it's threat to mark it zero" do
    (1..20).each { @game.roll(20) }
    @game.score.must_equal 0
  end

  it "can score a game when given an array of rolls" do
    @game.score(Array.new(12, 10)).must_equal 300
  end

  it "can score a game full of people who are about to enter a world of pain" do
    rolls = [-4, 'a', [], nil, 32, Object.new, Hash.new, 20, 3.14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    @game.score(rolls).must_equal 0
  end

  it "won't calculate the score for a game if you aren't done bowling yet" do
    (1..12).each { @game.roll(3) }
    @game.score.must_equal "You aren't done bowling yet"
  end
end
