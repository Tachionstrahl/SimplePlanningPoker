/**
 * Interface for a participant
 */
export interface IParticipant {
    /**
     * Name of the participant
     */
    name: string;
    /**
     * Flag indicating if the participant has estimated
     */
    estimated: boolean;
    /**
     * The estimation of the participant
     * Undefined if not estimated
     * @example "8"
     */
    estimation?: string;
}