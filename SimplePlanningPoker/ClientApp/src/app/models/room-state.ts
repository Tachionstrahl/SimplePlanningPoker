import { ICardSet } from "./card-set";
import { IParticipant } from "./participant";

export interface IRoomState {
    /**
     * The ID of the room
     */
    roomId: string;

    roomStateName: string;
    /**
     * The participants in the room
     */
    participants: IParticipant[];
    
    estimates: Record<string, string>;

    cardSet: ICardSet;
    
}