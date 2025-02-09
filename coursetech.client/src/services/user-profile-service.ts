import { RouteConstants } from "../constants/route-constants";
import { UpdateUserProfileDto } from "../types/dto/userProfile/update-user-profile-dto";
import { UserProfileDto } from "../types/dto/userProfile/user-profile-dto";
import { BaseResult } from "../types/result/base-result";
import { DataResult } from "../types/result/data-result";
import { ApiClient } from "./api-client";

export class UserProfileService {
    private apiClient: ApiClient;

    constructor(baseUrl: string) {
        this.apiClient = new ApiClient(baseUrl);
    }

    public async getUserProfileAsync(userId : string): Promise<DataResult<UserProfileDto>> {
        return this.apiClient.get<DataResult<UserProfileDto>>(RouteConstants.GET_USER_PROFILE, { userId: userId });
    }

    public async updateUserProfileAsync(dto: UpdateUserProfileDto): Promise<BaseResult> {
        return this.apiClient.put<BaseResult>(RouteConstants.UPDATE_USER_PROFILE, dto);
    }
}
