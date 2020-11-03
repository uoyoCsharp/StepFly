export interface UserState {
    userId: string | undefined;
    name: string | undefined;
    accessToken: string | undefined;
    roles: string | undefined;
    isLockout: boolean | undefined;
    lastLoginTime: number;
    platform: 'lexin' | 'xiaomi';
}