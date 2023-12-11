import {PagedList, ProjectListItemDto} from "../../domain/models";
import {v4 as uuidv4} from "uuid";

const initialProjectNames = ["MIRT", "BrokerVault", "Vanderbilt IAT", "Congenius", "Delta Dental", "MedeAnalytics", "Tivity RCM"];

const constructSampleData = (): PagedList<ProjectListItemDto> => {
    const listItems = initialProjectNames.map(name => ({id: uuidv4().toString(), name} as ProjectListItemDto));
    return {
        items: listItems,
        pageCount: 1,
        itemCount: listItems.length,
        page: 0,
        pageSize: listItems.length,
        totalCount: listItems.length};
};

export default constructSampleData();