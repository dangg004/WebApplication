export type PortfolioGet = {
  ID: number;
  symbol: string;
  companyName: string;
  purchase: number;
  lastDiv: number;
  industry: string;
  marketCap: number;
  comment: any;
};

export type PortfolioPost = {
  symbol: string;
};
