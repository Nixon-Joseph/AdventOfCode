import { inputParser } from "../solutionHelpers";

const Day5 = {
	sortUpdateByRules: (currentUpdate, allRules) => {
		const sorted = [...currentUpdate].sort((a, b) => {
			const foundARules = allRules.filter(
				(x) => x.left === a && currentUpdate.includes(x.right)
			);
			const foundBRules = allRules.filter(
				(x) => x.left === b && currentUpdate.includes(x.right)
			);
			return foundARules.length > foundBRules.length ? -1 : 1;
		});
		return sorted;
	},
	getMiddleNumberOfCorrectlySortedUpdate: (currentUpdate, allRules) => {
		const sorted = Day5.sortUpdateByRules(currentUpdate, allRules);
		if (sorted.join("") === currentUpdate.join("")) {
			return currentUpdate[Math.floor(currentUpdate.length / 2)];
		}
		return 0;
	},
	setup: (input) => {
		const sections = input.split("\r\n\r\n");
		const rules = inputParser.parseAsLeftRightArrays(
			sections[0],
			"|",
			true,
			true
		);
		const ruleArr = rules.left.map((v, i) => ({
			left: v,
			right: rules.right[i],
		}));
		const updates = inputParser.parseAsMultiDimentionArray(
			sections[1],
			",",
			true,
			true
		);
		return { ruleArr, updates };
	},
	part1: async (input) => {
		const { ruleArr, updates } = Day5.setup(input);
		let middleSum = 0;
		updates.forEach((update) => {
			middleSum += Day5.getMiddleNumberOfCorrectlySortedUpdate(update, ruleArr);
		});
		return middleSum;
	},
	part2: async (input) => {
		const { ruleArr, updates } = Day5.setup(input);
		let middleSum = 0;
		updates.forEach((update) => {
			if (Day5.getMiddleNumberOfCorrectlySortedUpdate(update, ruleArr) === 0) {
				const sortedUpdate = Day5.sortUpdateByRules(update, ruleArr);
				middleSum += sortedUpdate[Math.floor(sortedUpdate.length / 2)];
			}
		});
		return middleSum;
	},
};

export default Day5;
