import { BaseResult } from "./base-result";

export interface DataResult<T> extends BaseResult {
    data?: T;
}