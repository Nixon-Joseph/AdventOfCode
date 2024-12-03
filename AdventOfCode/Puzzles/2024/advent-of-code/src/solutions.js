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

const Day2 = {
	setup: (input) => {
		return inputParser.parseAsMultiDimentionArray(input);
	},
	checkBaseLevelSafe: (levels) => {
		let safeReport = true;
		let direction = 0;
		for (let i = 1; i < levels.length; i++) {
			const diff = levels[i] - levels[i - 1];
			// first iteration, determine direction
			if (Math.abs(diff) > 3) {
				// if the diff is too big, unsafe.
				safeReport = false;
				break;
			} else if (diff === 0) {
				// if they match, unsafe.
				safeReport = false;
				break;
			} else if (diff > 0) {
				// trending up
				if (direction === -1) {
					// if trending up but previously was trending down, unsafe.
					safeReport = false;
					break;
				}
				direction = 1;
			} else if (diff < 0) {
				// trending down
				if (direction === 1) {
					// if trending down but previously was trending up, unsafe.
					safeReport = false;
					break;
				}
				direction = -1;
			}
		}
		return safeReport;
	},
	part1: async (input) => {
		const inputArrs = Day2.setup(input);
		let safeReports = 0;
		inputArrs.forEach((levels) => {
			if (Day2.checkBaseLevelSafe(levels)) {
				safeReports++;
			}
		});
		return safeReports;
	},
	part2: async (input) => {
		const inputArrs = Day2.setup(input);
		let safeReports = 0;
		inputArrs.forEach((levels) => {
			let safe = Day2.checkBaseLevelSafe(levels);
			let index = 0;
			while (!safe && index < levels.length) {
				const newLevels = [...levels];
				newLevels.splice(index, 1);
				safe = Day2.checkBaseLevelSafe(newLevels);
				index++;
			}
			if (safe) {
				safeReports++;
			}
		});
		return safeReports;
	},
};

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

export const Solutions = {
	Day1,
	Day2,
	Day3,
};
