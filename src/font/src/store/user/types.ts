export interface UserState {
    name: string | undefined;
    accessToken: string | undefined;
    roles: string | undefined;
    isLockout: boolean | undefined;
    lastLoginTime: number;
}