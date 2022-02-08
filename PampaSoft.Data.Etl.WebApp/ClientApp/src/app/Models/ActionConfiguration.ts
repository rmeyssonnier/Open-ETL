import {ActionType} from "./Action";

export type ActionConfigurationTypes = ExtractHttpConfiguration |
  ExtractMinioConfiguration |
  TransformRenameConfiguration |
  TransformKeepConfiguration |
  TransformRemoveConfiguration |
  LoadMinioConfiguration |
  LoadPostgresConfiguration;

export interface ActionConfiguration {
  type: ActionType;
  actionId: number;
  configuration?: ActionConfigurationTypes;
}

export enum DataFormat {
  JSON= "JSON",
  CSV = "CSV",
  XML = "XML"
}

export abstract class BaseConfiguration {
  abstract getDescription(): string;
}

export class ExtractHttpConfiguration extends BaseConfiguration {
  url: string = '';
  format: DataFormat = DataFormat.JSON;
  csvFormat?: {colDelimiter: string, lineDelimiter: string, haveHeader: boolean}

  constructor(props:any = undefined) {
    super();
    if (props) {
      this.url = props.url;
      this.format = props.format;
      this.csvFormat = props.csvFormat;
    }
  }

  getDescription(): string {
    let hostname = '';

    try {
      hostname = new URL(this.url).hostname;
    } catch (e: any) {
      hostname = this.url;
    }

    return "From <span>" + hostname + "</span> in <span>" + this.format + "</span>";
  }
}

export class ExtractMinioConfiguration extends BaseConfiguration {
  url: string = '';
  accessKey: string = '';
  secretKey: string = '';
  bucket: string = '';
  prefix: string = '';

  constructor(props:any = undefined) {
    super();
    if (props) {
      this.url = props.url;
      this.accessKey = props.accessKey;
      this.secretKey = props.secretKey;
      this.bucket = props.bucket;
      this.prefix = props.prefix;
    }
  }

  getDescription(): string {
    return "From <span>" + this.url + "</span> in <span>" + this.bucket + "</span>";
  }
}

export class TransformRenameConfiguration extends BaseConfiguration {
  newNames: {oldName: string, newName: string}[] = [];

  constructor(props:any = undefined) {
    super();
    if (props) {
      this.newNames = props.newNames;
    }
  }

  getDescription(): string {
    return "Rename <span>" + this.newNames.length + "</span> columns";
  }
}

export class TransformKeepConfiguration extends BaseConfiguration {
  toKeep: string[] = [];

  constructor(props:any = undefined) {
    super();
    if (props) {
      this.toKeep = props.toKeep;
    }
  }

  getDescription(): string {
    return "Keep only <span>" + this.toKeep.join(", ") + "</span>";
  }
}

export class TransformRemoveConfiguration extends BaseConfiguration {
  toRemove: string[] = [];

  constructor(props:any = undefined) {
    super();
    if (props) {
      this.toRemove = props.toRemove;
    }
  }

  getDescription(): string {
    return "Remove <span>" + this.toRemove.join(", ") + "</span>";
  }
}

export class LoadMinioConfiguration extends BaseConfiguration {
  url: string = '';
  accessKey: string = '';
  secretKey: string = '';
  bucket: string = '';
  path: string = '';

  constructor(props:any = undefined) {
    super();
    if (props) {
      this.url = props.url;
      this.accessKey = props.accessKey;
      this.secretKey = props.secretKey;
      this.bucket = props.bucket;
      this.path = props.prefix;
    }
  }

  getDescription(): string {
    return "To <span>" + this.url + "</span> in <span>" + this.bucket + "</span>";
  }
}

export class LoadPostgresConfiguration extends BaseConfiguration {
  url: string = '';
  username: string = '';
  password: string = '';
  database: string = '';

  constructor(props:any = undefined) {
    super();
    if (props) {
      this.url = props.url;
      this.username = props.username;
      this.password = props.password;
      this.database = props.database;
    }
  }

  getDescription(): string {
    return "To <span>" + this.url + "</span> in <span>" + this.database + "</span>";
  }
}

export function loadConfiguration(configuration: any) : ActionConfigurationTypes {
  switch (configuration.type as ActionType) {
    case ActionType.extractHttp:
      return new ExtractHttpConfiguration(configuration.configuration);
    case ActionType.extractMinio:
      return new ExtractMinioConfiguration(configuration.configuration);
    case ActionType.removeColumns:
      return new TransformRemoveConfiguration(configuration.configuration)
    case ActionType.renameColumns:
      return new TransformRenameConfiguration(configuration.configuration)
    case ActionType.keepColumns:
      return new TransformKeepConfiguration(configuration.configuration)
    case ActionType.loadMinio:
      return new LoadMinioConfiguration(configuration.configuration)
    case ActionType.loadPostgres:
      return new LoadPostgresConfiguration(configuration.configuration)
  }
}
