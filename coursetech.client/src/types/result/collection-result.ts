import { BaseResult } from "./base-result";

export interface CollectionResult<T> extends BaseResult {
    count: number; 
    data: T[];     
}