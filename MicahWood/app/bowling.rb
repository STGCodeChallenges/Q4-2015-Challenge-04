class Bowling

  def initialize
    @rolls = []
  end

  def roll(pins)
    if you_were_over_the_line(pins)
      print "MARK IT ZERO!"
      pins = 0
    end
    @rolls << pins
  end

  def score(rolls = nil)
    if rolls
      # play out each roll for validation purposes
      rolls.each { |r| roll(r) }
    end
    score = 0
    roll = 0

    (1..10).each do |frame|
      return "You aren't done bowling yet" if @rolls[roll].nil?
      if is_strike(roll)
        score += 10 + @rolls[roll + 1] + @rolls[roll + 2]
        roll += 1
      elsif is_spare(roll)
        score += 10 + @rolls[roll + 2]
        roll += 2
      else
        score += @rolls[roll] + @rolls[roll + 1]
        roll += 2
      end
    end
    score
  end

  private
    def is_spare(roll)
      @rolls[roll] + @rolls[roll + 1] == 10
    end

    def is_strike(roll)
      @rolls[roll] == 10
    end

    def you_were_over_the_line(pins)
      (! pins.is_a? Integer) || pins > 10 || pins < 0
    end

end
