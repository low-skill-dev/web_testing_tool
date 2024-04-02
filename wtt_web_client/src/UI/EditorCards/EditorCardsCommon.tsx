export default interface EditorCardButtons {
	MoveRightCallback: (actionGuid: string) => void;
	MoveLeftCallback: (actionGuid: string) => void;
	MoveDownCallback: (actionGuid: string) => void;
	MoveUpCallback: (actionGuid: string) => void;
}
