import { useEffect, useState } from "react";
import { Solutions } from "./solutions";
import useLocalStorageState from "use-local-storage-state";

const Solution = ({ day }) => {
	let [inputText, setInputText] = useState("");
	let [solution, setSolution] = useState(null);
	let [solutionPart1, setSolutionPart1] = useLocalStorageState(
		`day${day}solutionPart1`,
		{
			defaultServerValue: null,
		}
	);
	let [solutionPart2, setSolutionPart2] = useLocalStorageState(
		`day${day}solutionPart2`,
		{
			defaultServerValue: null,
		}
	);
	let [solutionPart1Time, setSolutionPart1Time] = useLocalStorageState(
		`day${day}solutionPart1Time1`,
		{
			defaultServerValue: null,
		}
	);
	let [solutionPart2Time, setSolutionPart2Time] = useLocalStorageState(
		`day${day}solutionPart1Time2`,
		{
			defaultServerValue: null,
		}
	);

	useEffect(() => {
		fetch(`/Inputs/Day${day}.txt`)
			.then((res) => res.text())
			.then((text) => {
				if (text.includes("<!DOCTYPE html>")) {
					setInputText("No Input");
				} else {
					setInputText(text);
				}
			})
			.catch((e) => console.error(e));
		const sol = Solutions[`Day${day}`] ?? null;
		setSolution(sol);
	}, [day]);

	const showSolution = async (part) => {
		if (
			solution &&
			((solution.part1 && part === 1) || (solution.part2 && part === 2))
		) {
			const startTime = performance.now();
			let result = null;
			if (part === 1) {
				result = await solution.part1(inputText);
				setSolutionPart1(result);
			} else {
				result = await solution.part2(inputText);
				setSolutionPart2(result);
			}
			const endTime = performance.now();
			console.log(`Part ${part} took ${endTime - startTime}ms`);
			if (part === 1) {
				setSolutionPart1Time(endTime - startTime);
			} else {
				setSolutionPart2Time(endTime - startTime);
			}
		} else {
			alert("No solution yet");
		}
	};

	return (
		<>
			<div className="row h-25">
				<div className="row text-center">
					<div className="col-6">
						<h2>Part 1:</h2>
						<button
							onClick={async () => {
								await showSolution(1);
							}}
						>
							{solution && solution.part1
								? "Show solution part 1"
								: "No solution yet"}
						</button>
						<br />
						{solutionPart1 ? (
							<input type="readonly" readOnly={true} value={solutionPart1} />
						) : null}
						{solutionPart1Time !== undefined ? (
							<pre>Part 1 took {solutionPart1Time}ms</pre>
						) : null}
					</div>
					<div className="col-6">
						<h2>Part 2:</h2>
						<button
							onClick={async () => {
								await showSolution(2);
							}}
						>
							{solution && solution.part2
								? "Show solution part 2"
								: "No solution yet"}
						</button>
						<br />
						{solutionPart2 ? (
							<input type="readonly" readOnly={true} value={solutionPart2} />
						) : null}
						{solutionPart2Time !== undefined ? (
							<pre>Part 2 took {solutionPart2Time}ms</pre>
						) : null}
					</div>
				</div>
			</div>
			<div className="h-50">
				<h2>Input:</h2>
				<textarea
					style={{
						maxHeight: "100%",
						height: "100%",
						width: "100%",
						resize: "none",
						fontFamily: "monospace",
					}}
					readOnly
					value={inputText || "No Input Text"}
				></textarea>
			</div>
		</>
	);
};

export default Solution;
