require 'json'
require 'net/http'
require 'securerandom'


class GetComplexity
  @@difficulty_selection = nil
  $api_key = nil
  @@file_path = File.join(Dir.getwd, 'api_key.txt')

  def self.main
    unless File.exist?(@@file_path)
      print 'Enter API key: '
      $api_key = gets.chomp
      File.write(@@file_path, $api_key)
    else
      $api_key = File.read(@@file_path)
    end

    puts 'Select a complexity level, 1-4. I for info.'
    reply = gets.chomp

    if reply.upcase == 'I'
      puts "1 = 1 word, min 8 characters, 1 uppercase, rest lowercase, 1 number, 1 special char
2 = 1 word, min 10 characters, 2 uppercase, rest lower, 2 numbers, 2 special char
3 = 2 words, min 12 char, 2 uppercase per word, rest lower, 3 numbers, 3 special char
4 = 2 words, min 14 char, 3 uppercase per word, rest lower, 4 numbers, 4 special char"
      main
    end

    while !reply.to_i.between?(1, 4)
      print 'Please enter a valid response: '
      reply = gets.chomp
    end

    @@difficulty_selection = reply.to_i
    random_assembly = RandomAssembly.new
    random_assembly.ra
    @@difficulty_selection
  end
    def self.difficulty_selection
    @@difficulty_selection
  end
end

class RandomAssembly
  def ra
    print "Do you want the order of the password components to be randomized? Type 'y' or 'n': "
    yesno = gets.chomp.downcase

    while !['y', 'n'].include?(yesno)
      print 'Please enter a valid response: '
      yesno = gets.chomp.downcase
    end

    wants_random = yesno == 'y'
    puts 'Creating...'

    choose_path = ChoosePath.new
    choose_path.func_loop(GetComplexity.class_variable_get(:@@difficulty_selection), wants_random)
  end
end

module Fluff
  def self.rand_nums(num_int_generations)
    (1..num_int_generations).map { rand(1..9) }
  end

  def self.rand_alph(num_alph_generations)
    alphabet = ('a'..'z').to_a
    (1..num_alph_generations).map { alphabet.sample }
  end

  def self.rand_sym(num_sym_generations)
    #%w is shorthand for an array of strings
    symbols = %w[~ ` ! @ # $ % ^ & * ( ) _ - + = { [ } ] | : ; " < > . ? /]
    (1..num_sym_generations).map { symbols.sample }
  end
end

class ChoosePath
  def make_readable
    word = GetWord.word_fetch
    while word.length < 5
      make_readable
    end
    return word
  end

  def func_loop(complexity, is_random)
    pass_components = []

    (complexity > 2 ? 2 : 1).times do
      word = make_readable.downcase
      end_index = [word.length, complexity % 4 + 1].min
      word[-end_index..-1] = word[-end_index..-1].upcase
      pass_components << word
    end
case complexity
  when 1
    pass_components += Fluff.rand_nums(1).map(&:to_s)
    pass_components += Fluff.rand_sym(1).map(&:to_s)
  when 2
    pass_components += Fluff.rand_nums(2).map(&:to_s)
    pass_components += Fluff.rand_sym(2).map(&:to_s)
  when 3
    pass_components += Fluff.rand_nums(3).map(&:to_s)
    pass_components += Fluff.rand_sym(3).map(&:to_s)
  when 4
    pass_components += Fluff.rand_nums(4).map(&:to_s)
    pass_components += Fluff.rand_sym(4).map(&:to_s)
end

if is_random
  pass_components.shuffle!
  password = pass_components.join
  Filler.add_filler(password)
  return password
else
  password = pass_components.join
  Filler.add_filler(password)
  return password
end
end
end

class Filler
  def self.add_filler(password)
    min_length = case GetComplexity.difficulty_selection
      when 1 then 8
      when 2 then 10
      when 3 then 12
      when 4 then 14
    end

    while password.length < min_length
      case SecureRandom.random_number(3)
      when 0
        password += Fluff.rand_nums(1).map(&:to_s).join
      when 1
        password += Fluff.rand_alph(1).map(&:to_s).join
      when 2
        password += Fluff.rand_sym(1).map(&:to_s).join
      end
    end

    puts(password)
    Finish.end()
  end
end

class Finish
  def self.end
    puts 'Again? (y if yes)'
    restart = gets.chomp
    if restart == 'y'
      GetComplexity.main
    else
      exit()
    end
  end
end

class GetWord
  def self.word_fetch
    uri = URI('https://api.api-ninjas.com/v1/randomword')
    request = Net::HTTP::Get.new(uri)
    request['X-Api-Key'] = $api_key

    http = Net::HTTP.new(uri.host, uri.port)
    http.use_ssl = true
    http.read_timeout = 10

    response = http.request(request)

    if response.code == '200'
      JSON.parse(response.body)['word']
    else
      puts "Something went wrong with the API: #{response.code}"
      GetComplexity.main
      return 'retrying'
    end
  end
end

GetComplexity.main()
