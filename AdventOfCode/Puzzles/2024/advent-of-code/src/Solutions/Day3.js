const Day3 = {
	part1: async (input) => {
		const foundMatches = input.match(/mul\([0-9]{1,3},\s?[0-9]{1,3}\)/g);
		let sum = 0;
		foundMatches.forEach((mul) => {
			const [a, b] = mul.match(/[0-9]{1,3}/g);
			sum += a * b;
		});
		return sum;
	},
	part2: async (input) => {
		let modifiedInput = input;
		const dontRegex = /don't\(\)/;
		while (dontRegex.test(modifiedInput)) {
			const nextDont = modifiedInput.match(dontRegex);
			const nextDo = modifiedInput.indexOf("do()", nextDont.index + 6);
			modifiedInput =
				modifiedInput.substring(0, nextDont.index) +
				modifiedInput.substring(nextDo + 4);
		}
		return Day3.part1(modifiedInput);
	},
};

export default Day3;
