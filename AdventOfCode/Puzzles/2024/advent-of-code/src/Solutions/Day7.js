import { inputParser } from "../solutionHelpers";

const Day7 = {
	setup: (input) => {
		const lines = inputParser.parseAsLineArray(input);
		const calibrations = lines.map((line) => {
			const parts = line.split(" ");
			return {
				value: Number(parts[0].replace(":", "").trim()),
				numbers: parts.slice(1).map(Number),
			};
		});
		return calibrations;
	},
	generateCombinations: (arr, combos, possibleOperators, m = []) => {
		if (m.length === arr.length) {
			combos.push(m);
			return;
		}
		possibleOperators.forEach((op) => {
			Day7.generateCombinations(arr, combos, possibleOperators, m.concat(op));
		});
	},
	add: (a, b) => a + b,
	multiply: (a, b) => a * b,
	combine: (a, b) => Number(a.toString() + b.toString()),
	solveForCorrectSum: (calibrations, possibleOperators) => {
		let correctSum = 0;
		calibrations.forEach((calibration) => {
			const valueToMatch = calibration.value;
			const operators = Array.from(
				{ length: calibration.numbers.length - 1 },
				() => Day7.add
			);
			const possibleCombinations = [];
			Day7.generateCombinations(
				operators,
				possibleCombinations,
				possibleOperators
			);
			for (
				let comboIndex = 0;
				comboIndex < possibleCombinations.length;
				comboIndex++
			) {
				const currentCombo = possibleCombinations[comboIndex];
				let total = currentCombo.reduce(
					(acc, op, i) => op(acc, calibration.numbers[i + 1]),
					calibration.numbers[0]
				);
				if (total === valueToMatch) {
					correctSum += valueToMatch;
					break;
				}
			}
		});
		return correctSum;
	},
	part1: async (input) => {
		const calibrations = Day7.setup(input);
		const possibleOperators = [Day7.add, Day7.multiply];
		const correctSum = Day7.solveForCorrectSum(calibrations, possibleOperators);
		return correctSum;
	},
	part2: async (input) => {
		const calibrations = Day7.setup(input);
		const possibleOperators = [Day7.add, Day7.multiply, Day7.combine];
		const correctSum = Day7.solveForCorrectSum(calibrations, possibleOperators);
		return correctSum;
	},
};

export default Day7;
