import { IQuestionDto } from "./question-dto-interface";
import { TestVariantDto } from "./test-variant-dto";

export interface OpenQuestionDto extends IQuestionDto { }

export interface PracticalQuestionDto extends IQuestionDto { }

export interface TestQuestionDto extends IQuestionDto {
    testVariants: TestVariantDto[];
}