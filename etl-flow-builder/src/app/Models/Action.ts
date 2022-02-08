export enum ActionType {
  extractHttp = "Extract HTTP",
  extractMinio = "Extract Minio",
  renameColumns = "Rename columns",
  keepColumns = "Keep columns",
  removeColumns = "Remove columns",
  loadMinio = "Load minio",
  loadPostgres = "Load postgres",
}

export enum ActionCategory {
  extract = 'Extract',
  transform = 'Transform',
  load = 'Load',
}

export interface Action {
  id: string;
  type: ActionType;
  category: ActionCategory;
  icon: string;
  description: string;
}

export const actionSet: Action[] = [
  {
    id: getUniqueId(),
    category: ActionCategory.extract,
    type: ActionType.extractHttp,
    icon: 'assets/action.svg',
    description: 'Extract data from HTTP source'
  },
  {
    id: getUniqueId(),
    category: ActionCategory.extract,
    type: ActionType.extractMinio,
    icon: 'assets/action.svg',
    description: 'Extract data from S3 or minio bucket'
  },
  {
    id: getUniqueId(),
    category: ActionCategory.transform,
    type: ActionType.renameColumns,
    icon: 'assets/action.svg',
    description: 'Rename some columns of datatable'
  },
  {
    id: getUniqueId(),
    category: ActionCategory.transform,
    type: ActionType.keepColumns,
    icon: 'assets/action.svg',
    description: 'Keep only a list of columns'
  },
  {
    id: getUniqueId(),
    category: ActionCategory.transform,
    type: ActionType.removeColumns,
    icon: 'assets/action.svg',
    description: 'Remove a list of columns'
  },
  {
    id: getUniqueId(),
    category: ActionCategory.load,
    type: ActionType.loadMinio,
    icon: 'assets/action.svg',
    description: 'Load datatable to bucket as CSV'
  },
  {
    id: getUniqueId(),
    category: ActionCategory.load,
    type: ActionType.loadPostgres,
    icon: 'assets/action.svg',
    description: 'Create a new table into postgres and save datatable'
  },
]

export function getUniqueId(): string {
  const stringArr = [];
  for(let i = 0; i < 4; i++){
    // tslint:disable-next-line:no-bitwise
    const S4 = (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    stringArr.push(S4);
  }
  return stringArr.join('-');
}

export function getPlacedElement(type: string) {
  const foundType = actionSet.find(action => action.id === type);
  return `<div class="blockyleft">
    <img src="${foundType!.icon}">
      <p class="blockyname">${foundType!.type}</p>
    </div>
    <div class="blockyright">
      <img src="assets/more.svg">
    </div>
    <div class="blockydiv"></div>
    <div class="blockyinfo"></div>`;
}
