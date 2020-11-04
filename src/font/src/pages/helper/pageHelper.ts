export default class PageHelper {
    public static getPlatformCode(platform: any) {
        return platform === 'lexin' ? 0 : 1;
    }

    public static getStepTagColor(stepNum: number) {
        if (stepNum > 0 && stepNum <= 10000) {
            return 'primary'
        } else if (stepNum > 10000 && stepNum <= 20000) {
            return 'danger'
        }
        else if (stepNum > 20000 && stepNum <= 40000) {
            return 'light-green';
        }
        else if (stepNum > 40000) {
            return 'light-orange';
        }

        return 'light-brownish';
    }
}