const inputParser = {
	parseAsMultiDimentionArray: (
		input,
		splitChar = " ",
		removeEmpties = true
	) => {
		return input.split("\r\n").map((line) =>
			line
				.split(splitChar)
				.filter((item) => (removeEmpties ? item !== "" : true))
				.map((x) => x.trim())
		);
	},
	parseAsLeftRightArrays: (input, splitChar = " ", removeEmpties = true) => {
		const returnArrs = { left: [], right: [] };
		input.split("\r\n").forEach((line) => {
			const lineContents = line
				.split(splitChar)
				.filter((item) => (removeEmpties ? item !== "" : true))
				.map((x) => x.trim());
			if (lineContents.length === 2) {
				returnArrs.left.push(lineContents[0]);
				returnArrs.right.push(lineContents[1]);
			}
		});
		return returnArrs;
	},
};

const Day1 = {
	setup: (input) => {
		const { left, right } = inputParser.parseAsLeftRightArrays(input);
		left.sort();
		right.sort();
		return { left, right };
	},
	part1: async (input) => {
		const { left, right } = Day1.setup(input);
		let total = 0;
		for (let i = 0; i < left.length; i++) {
			const diff = Math.abs(left[i] - right[i]);
			total += diff;
		}
		return total;
	},
	part2: async (input) => {
		const { left, right } = Day1.setup(input);
		let simScore = 0;
		left.forEach((item) => {
			const foundInRight = right.filter((x) => x === item);
			simScore += item * foundInRight.length;
		});
		return simScore;
	},
};

const Day2 = { part1: undefined, part2: undefined };

export const Solutions = {
	Day1,
	Day2,
};
