import React from "react";
import { testIncomeStatementData } from "./testData";
const data = testIncomeStatementData;
type Props = {};

type Company = (typeof data)[0];

const configs = [
  {
    Label: "Year",
    render: (company: Company) => company.calendarYear,
  },
  {
    Label: "Cost of revenue",
    render: (company: Company) => company.costOfRevenue,
  },
];

const Table = (props: Props) => {
  const renderedRow = data.map((company) => {
    return (
      <tr key={company.cik}>
        {configs.map((val: any) => {
          return (
            <td className="p-4 whitespace-nowrap text-sm font-normal text-gray-900">
              {val.render(company)}
            </td>
          );
        })}
      </tr>
    );
  });
  const renderedHeader = configs.map((config: any) => {
    return (
      <th
        className="p-4 text-left text-xs font-medium text-fray-500 uppercase tracking-wider"
        key={config.Label}
      >
        {config.Label}
      </th>
    );
  });
  return (
    <div className="bg-white shadow rounded-lg p-4 sm:p-6 xl:p-8">
      <table className="min-w-full divide-y divide-gray-200 m-5">
        <thead className="bg-gray-50">{renderedHeader}</thead>
        <tbody>{renderedRow}</tbody>
      </table>
    </div>
  );
};

export default Table;
