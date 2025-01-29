import { Error } from "./error.ts";

export interface BaseResult {
    error: Error;
    isSuccess: boolean;
}