import { BindingTypeModel } from '@/models/apiModel';

export interface HomeState {
    readedNotices: number[] | undefined;
    bindType: BindingTypeModel | undefined;
    version: string | undefined;
}