# Pour l'instant, lance python dans un terminal.
# Fait Soit :
#	"from ngrams import *" 
# Si t'as lance python dans le meme repertoire que ce fichier
# ou 
# copie colle le fichier dans le terminal
# puis tappe 
#
#	create_grams_from_file("tonfichier_messages.txt")
#
# Ca va creer 3 fichiers text dans le repertoire courant



#Choix arbitraire, on peut juste l'augmenter
MAX_GRAM_LENGTH = 6
#Liste de listes vides
all_grams= [[] for x in range(MAX_GRAM_LENGTH+1)]
sentence_end_markers = ['.', '!', '...', '?']

##	@fn	create_n_grams(n,line)
#	Split le message recu aux espaces.
#	Pour chaque ensemble de taille n de mots
#	consecutifs, creer un gram ou incrementer son compte.
def create_n_grams(n,line):
	words = line.split()
	for i in range(1+len(words)):
		if i+n<=len(words):
			sub=' '.join(words[i:i+n])
			if sub is not None:
				#Le try catch c'est car en Python
				#  "It's better to as for forgivness
				#	than ask for permission"
				try:
					i = all_grams[n].index(sub)
					all_grams[n][i].count+=1
				except ValueError:
					all_grams[n].append(Gram(sub))

## @fn create_starter_grams(line):
#	Create grams from first words of sentences.
#
def create_starter_grams(line):
	words = line.split()
	subs = [words[0]]
	prev_is_end_of_sentence = False
	for word in words:
		if prev_is_end_of_sentence:
			subs.append(word)
			prev_is_end_of_sentence = False
			continue
		if word in sentence_end_markers:	
			prev_is_end_of_sentence = True	
	for sub in subs:
		try:
			i = all_grams[0].index(sub)
			all_grams[0][i].count+=1
		except ValueError:
			all_grams[0].append(Gram(sub))
			

##	@class	Gram
#	Un gram (sequence de mots) d'une certaine longueur
#	et le nombre d'occurence total de ce gram dans le fichier
#	source/corpus.
#
class Gram:
	def __init__(self,_str):
		self.value = _str.lower()
		self.count = 1
	
	#Ignores caps
	def __eq__(self, other):
		if type(other) is str:
			return self.value == other.lower()
		else:
			return self.value == other.value
	
	def __str__(self):
		res = self.value + "," + str(self.count)
		return res

def create_grams_from_file(file_path):
	#Read from file then create and count grams
	with open(file_path, encoding="utf8") as f:
		for line in f:
			for n in range(2,MAX_GRAM_LENGTH+1):              
				create_n_grams(n, line)
			create_starter_grams(line)
	#Sort from high to low
	for grams in all_grams:
		grams.sort(key=lambda x: x.count, reverse=True)
	
	#Write results to file
	for i in range(0,MAX_GRAM_LENGTH+1):
		with open('perso_'+str(i)+'grams.txt','w', encoding="utf8") as f:
			for l in all_grams[i]:
				f.write(str(l)+'\n')
		
