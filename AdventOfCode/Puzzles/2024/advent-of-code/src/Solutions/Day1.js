import { inputParser } from "../solutionHelpers";

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

export default Day1;
